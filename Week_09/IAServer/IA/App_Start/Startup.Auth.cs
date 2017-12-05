using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using IA.Providers;
using IA.Models;
// added...
using Microsoft.AspNet.Identity.Owin;

namespace IA
{
    public partial class Startup
    {

        // Provides information needed to control Authorization Server middleware behavior
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        // OAuth value, a client (app) identifier
        public static string PublicClientId { get; private set; }

        // FYI - Info about static properties...
        // https://msdn.microsoft.com/en-us/library/w86s7x04.aspx#Anchor_2

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request

            // Initialize the data context (and therefore Entity Framework and a database connection)
            app.CreatePerOwinContext(ApplicationDbContext.Create);

            // Initialize the ASP.NET Identity user manager object (which is used elsewhere in the app)
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            // Initialize the ASP.NET Identity sign in manager object (which is used elsewhere in the app)
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Initialize claims for the app
            IdentityInitialize.LoadAppClaims();
            
            // Initialize user accounts for the app
            IdentityInitialize.LoadUserAccounts();

            // Enable the application to use a cookie to store information for the signed in user
            // Here, configure the cookie's options (there are other options too)
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                CookieName = "senecaict786a20e4b6c82f2b",
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromDays(14),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager, DefaultAuthenticationTypes.ApplicationCookie))
                }
            });

            // Enable the application to use a cookie to temporarily store 
            // information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            // Here, configure the access token's options (there are other options too)
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true
            };

            // The following command does two very important tasks...
            app.UseOAuthBearerTokens(OAuthOptions);
            // First...
            // Enable the app to ISSUE "Bearer" access tokens
            // Configure the /token endpoint to listen for requests for access tokens
            // Second...
            // Enable the app to VALIDATE "Bearer" access tokens
            // Can extract (read) an access token in the "Authorization" request header
            // If valid, then it can create an IPrincipal and attach it to the request thread

            // In summary, the one method above - UseOAuthBearerTokens - essentially runs
            // two separate methods...
            // UseOAuthAuthorizationServer() - authenticate credentials, issue a cookie or token
            // UseOAuthBearerAuthentication() - validate an access token (in an incoming request)
            // Reference:
            // http://stackoverflow.com/a/28049897

            // More info...
            // UseOAuthAuthorizationServer() - authenticate credentials, issue a cookie or token
            // Adds OAuth2 Authorization Server capabilities to an OWIN web application. 
            // This middleware performs the request processing for the Authorize and Token endpoints,
            // which are defined by the OAuth2 specification.
            // Reference:
            // https://msdn.microsoft.com/en-us/library/owin.oauthauthorizationserverextensions.useoauthauthorizationserver(v=vs.113).aspx
            // UseOAuthBearerAuthentication() - validate an access token (in an incoming request)
            // Adds Bearer token processing to an OWIN application pipeline. 
            // This middleware understands appropriately formatted and secured tokens 
            // which appear in the request header. The claims within the token are added to the 
            // current request's IPrincipal User.
            // Reference:
            // https://msdn.microsoft.com/en-us/library/owin.oauthbearerauthenticationextensions.useoauthbearerauthentication(v=vs.113).aspx

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}
