using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
// added...
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.DataHandler;
using System.Web.Security;

namespace IA.Controllers
{
    // This controller will return info inside the access token
    // Obviously, you must include an Authorization header
    // As stated in the class notes, the access token does not hold any secrets

    /// <summary>
    /// Delivers information about the authenticated user
    /// </summary>
    [Authorize]
    public class AuthInfoController : ApiController
    {
        // GET: api/AuthInfo
        /// <summary>
        /// Detailed information about the authenticated user
        /// </summary>
        /// <returns>Identity and claims data</returns>
        public IDictionary<string, string> Get()
        {
            // Get a reference to the OWIN context
            var owin = Request.GetOwinContext();

            // Get the authorization header value
            var authHeader = owin.Request.Headers["Authorization"];
            // Remove the word/prefix "Bearer"
            authHeader = authHeader.Replace("Bearer ", "");

            // Declare the kind of secure ticket data format that we have
            var secureTicket = new TicketDataFormat(new MachineKeyProtector());
            // Unprotect (decode/decrypt) the secure ticket
            AuthenticationTicket ticket = secureTicket.Unprotect(authHeader);

            // Now we can go through the ticket, and extract its contents...

            var ticketValues = new Dictionary<string, string>();

            // Get the basics from the identity object

            ticketValues.Add("Authentication type", ticket.Identity.AuthenticationType);
            ticketValues.Add("Name", ticket.Identity.Name);
            ticketValues.Add("Number of claims", ticket.Identity.Claims.Count().ToString());

            // Issuer(s)
            var issuers = ticket.Identity.Claims.Select(c => c.Issuer).Distinct();
            if (issuers.Count() == 1)
            {
                ticketValues.Add("Claim issuer", issuers.ElementAt(0));
            }
            else
            {
                for (int i = 0; i < issuers.Count(); i++)
                {
                    // Get the issuer
                    var issuer = issuers.ElementAt(i);

                    // Write it out
                    ticketValues.Add(string.Format("Claim issuer {0}", i), issuer);
                }
            }

            // Subject(s)
            var subjects = ticket.Identity.Claims.Select(c => c.Subject.ToString()).Distinct();
            if (subjects.Count() == 1)
            {
                ticketValues.Add("Claim subject", subjects.ElementAt(0));
            }
            else
            {
                for (int i = 0; i < subjects.Count(); i++)
                {
                    // Get the subject
                    var subject = subjects.ElementAt(i);

                    // Write it out
                    ticketValues.Add(string.Format("Claim subject {0}", i), subject);
                }
            }

            // Go through the claims

            for (int i = 0; i < ticket.Identity.Claims.Count(); i++)
            {
                // Get the claim
                var item = ticket.Identity.Claims.ElementAt(i);

                // Write it out
                var v = string.Format("{0}, {1}", item.Type, item.Value);
                ticketValues.Add(string.Format("Claim {0} type, value", i), v);
            }

            // Get the values from the properties object

            ticketValues.Add("Expires", ticket.Properties.ExpiresUtc.GetValueOrDefault().ToString());
            ticketValues.Add("Issued", ticket.Properties.IssuedUtc.GetValueOrDefault().ToString());

            // Go through the additional properties

            foreach (var item in ticket.Properties.Dictionary)
            {
                ticketValues.Add("Ticket properties - " + item.Key, item.Value);
            }

            return ticketValues;
        }
    }





    // Helper class, to decode/decrypt the access token
    // Credit goes to Stephen Long
    // https://long2know.com/2015/05/decrypting-owin-authentication-ticket/
    class MachineKeyProtector : Microsoft.Owin.Security.DataProtection.IDataProtector
    {
        private readonly string[] _purpose = { typeof(OAuthAuthorizationServerMiddleware).Namespace, "Access_Token", "v1" };

        public byte[] Protect(byte[] userData)
        {
            throw new NotImplementedException();
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return MachineKey.Unprotect(protectedData, _purpose);
        }
    }
}
