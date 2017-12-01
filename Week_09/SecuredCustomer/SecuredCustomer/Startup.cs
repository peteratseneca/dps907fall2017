using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SecuredCustomer.Startup))]

namespace SecuredCustomer
{
    // Attention 01 - Using the NuGet Package Manager, several packages were added to this project

    // Attention 02 - This Startup class was added; it is auto-detected when the app loads into memory

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Attention 03 - This app does NOT create (issue) bearer tokens, but understands how to read (decrypt) and use them
            app.UseOAuthBearerAuthentication(new Microsoft.Owin.Security.OAuth.OAuthBearerAuthenticationOptions());
        }
    }
}
