using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using IA.Models;

namespace IA.Providers
{
    // Implements OAuth "Authorization Server" behaviour in this app
    // Provides default behaviours, which are customized by the "override" methods

    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        /*
         * GrantResourceOwnerCredentials
         * 
         * Called when a request to the /token endpoint arrives 
         * with a "grant_type" of "password". 
         * 
         * If the web application supports the resource owner credentials grant type 
         * it must validate the context.Username and context.Password as appropriate. 
         * To issue an access token the context.Validated must be called with a 
         * new ticket containing the claims about the resource owner which should be 
         * associated with the access token.
         * 
         * Reference:
         * https://msdn.microsoft.com/en-us/library/microsoft.owin.security.oauth.oauthauthorizationserverprovider(v=vs.113).aspx
        */
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // Attempt to locate the user account
            // Its type is a user account as defined by ASP.NET Identity and its store provider (Entity Framework)
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            // Create an identity object for use with an access token (web service use)
            // This statement transforms the user account (above) into a ClaimsIdentity,
            // which is an identity object for the current authenticated user
            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);

            // Create an identity object for use with a cookie (web app use)
            // As above, this statement transforms the user account (above) into a ClaimsIdentity,
            // which is an identity object for the current authenticated user
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            // An OAuth access token is an AuthenticationTicket object
            // It is a packaging container for a ClaimsIdentity object, OAuth-compatible
            // Here, we create a ticket that's based on the ClaimsIdentity from above
            // First, we define some state/values that will be visible as ticket properties
            // They will get added soon, when the TokenEndpoint() method is called
            AuthenticationProperties properties = CreateProperties(user.UserName);

            // Next, we create a ticket package/container...
            // It will have an "access_token" property, with the encrypted ClaimsIdentity data (user info and claims)
            // It will also have other properties (e.g. token_type, .issued, .expires, etc., 
            // and "userName" for easy reading without the need to decrypt/decode the access_token
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            
            // Validates (i.e. sign in / login) the user, and...
            // Configure the Owin context with the ticket and marks it as as validated. IsValidated becomes true.
            // Eventually, the pipeline will serialize the ticket as the response
            context.Validated(ticket);

            // Sign in (login) the user, and...
            // Configure the Owin context with a cookie
            // Eventually, the pipeline will return the cookie in the response
            //
            // Reference:
            // https://msdn.microsoft.com/en-us/library/dn323856(v=vs.113).aspx
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        /*
         * Called at the final stage of a successful Token endpoint request. 
         * The Owin pipeline calls this method.
         * 
         * Adds the additional properties that were defined above
         * 
         * Reference:
         * https://msdn.microsoft.com/en-us/library/microsoft.owin.security.oauth.oauthauthorizationserverprovider(v=vs.113).aspx
        */
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        // Not used in this app
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            // That is defined in the OAuth specifications
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        // Not used in this app
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        // Convenience method used above in GrantResourceOwnerCredentials
        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}