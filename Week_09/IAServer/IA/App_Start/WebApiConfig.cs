using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace IA
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // This app has web app controllers (Controller), and web service controllers (ApiController)
            // It is possible to configure separate authentication (i.e. validation) modules
            // Yes, we want to do this

            // Turn off web server host authentication (i.e. cookie validation)
            // This does not affect a request to a web app controller that includes a cookie
            // References:
            // https://msdn.microsoft.com/en-us/library/dn314641(v=vs.118).aspx
            // https://brockallen.com/2013/10/27/host-authentication-and-web-api-with-owin-and-active-vs-passive-authentication-middleware/
            config.SuppressDefaultHostAuthentication();

            // Configure Web API to use only bearer token authentication
            // It will look for an "Authentication" header in the request
            // with a value of "Bearer" plus an encrypted/encoded access token
            // This statement adds the host authentication filter to the request-processing pipeline
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
