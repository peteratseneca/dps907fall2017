using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
// added...
using System.Web.Http.ExceptionHandling;

namespace AssociationsIntro
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Add ByteFormatter to the pipeline
            config.Formatters.Add(new ServiceLayer.ByteFormatter());

            // Add HandleError to the pipeline
            config.Services.Replace(typeof(IExceptionHandler), new ServiceLayer.HandleError());

            // Attention 11 - Configure a default controller 
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { controller = "Root", id = RouteParameter.Optional }
            );
        }
    }
}
