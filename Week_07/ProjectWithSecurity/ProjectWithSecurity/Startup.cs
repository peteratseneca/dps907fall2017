using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

// Attention 01 - Read this comment block to learn how the app begins to load
// If the app is not yet loaded in the server's memory,
// the ASP.NET runtime scans the /bin folder, looking for OWIN components
// (in Solution Explorer, open the "References" branch, to see what gets copied to the /bin folder)
// If found, it looks for the following attribute...
[assembly: OwinStartup(typeof(ProjectWithSecurity.Startup))]

// The OWIN components load, and then looks for a "Startup" class

namespace ProjectWithSecurity
{
    // By convention, the root of a project that uses ASP.NET identity
    // includes a Startup.cs source code file (this file),
    // which has a Startup class that has a split implementation
    // The other part is in the App_Start folder, in the Startup.Auth.cs source code file
    public partial class Startup
    {
        // By convention, the OWIN components instantiate the Startup class,
        // and call its Configuration method
        public void Configuration(IAppBuilder app)
        {
            // The ConfigureAuth() method is defined in the Startup.Auth.cs source code file
            ConfigureAuth(app);
        }
    }
}
