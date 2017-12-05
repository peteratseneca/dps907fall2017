using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.Security.Claims;
using System.ComponentModel;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace IA.Controllers
{
    // This is version 1 of a custom authorization attribute class
    // ####################
    // For an ApiController (web service, Web API)
    // ####################
    // When I have time, I'll rewrite it so that the Roles and Users properties are not included

    // This source code file can hold many custom authorize attribute subclasses
    // However, we start with the one shown below

    // For example, this can be added to a controller method:
    // [AuthorizeClaim(ClaimType = "GivenName", ClaimValue = "User")]

    public class AuthorizeClaim : AuthorizeAttribute
    {
        // Properties
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        // Override method
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            // Get a reference to the user
            var user = actionContext.RequestContext.Principal as ClaimsPrincipal;

            // Matches (below) are case-insensitive

            // Look for claims that match the incoming type
            // The matchingClaims will be a collection of zero or more matching claims
            var matchingClaims = user.Claims
                .Where(c => c.Type.ToLower().Contains(ClaimType.ToLower()));

            // Attempt to locate matching values
            var matchedClaim = false;
            foreach (var claim in matchingClaims)
            {
                if (claim.Value.ToLower() == ClaimValue.ToLower())
                {
                    matchedClaim = true;
                    break;
                }
            }

            if (matchedClaim)
            {
                // Yes, authorized
                base.OnAuthorization(actionContext);
            }
            else
            {
                // No, not authorized
                base.HandleUnauthorizedRequest(actionContext);
            }
        }

    }

}
