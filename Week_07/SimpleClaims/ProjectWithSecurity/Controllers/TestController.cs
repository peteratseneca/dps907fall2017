using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
// added...
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;

namespace ProjectWithSecurity.Controllers
{
    public class TestController : ApiController
    {
        // Attention 31 - This controller enables you to test authentication and role claims

        // How to use this controller:

        // 1. Load/run this app (in localhost)

        // 3. Create three or four accounts, and include roles
        //    Look at the role names (below) that are being tested

        // 4. Load/run Fiddler. 

        // 5. For each account, call the token endpoint to obtain a token;
        //    you can copy-paste them to a plain text file (and identify them)

        // 6. In Fiddler, execute requests that will test the accounts 
        //    by requesting this controller's resources 

        // GET: api/Test
        // Anonymous
        // Attention 32 - This method will display all claims for authenticated users
        public IEnumerable<string> Get()
        {
            // Container for user and claims info
            List<string> allClaims = new List<string>();

            // Is this request authenticated?
            allClaims.Add("Authenticated = " + (User.Identity.IsAuthenticated ? "Yes" : "No"));
            if (User.Identity.IsAuthenticated)
            {
                // Cast the generic principal to a claims-carrying identity
                var identity = User.Identity as ClaimsIdentity;
                // Extract only the claims
                var claims = identity.Claims
                    .Select(c => new { Type = c.Type, Value = c.Value })
                    .AsEnumerable();
                foreach (var claim in claims)
                {
                    // Create a readable string
                    allClaims.Add(claim.Type + " = " + claim.Value);
                }
            }

            return allClaims;
        }

        // GET: Test/UserList
        // Attention 33 - This will fetch the list of registered users
        [Route("api/test/userlist")]
        public IHttpActionResult GetUserList()
        {
            // Container to hold the user names
            var userList = new List<string>();

            // Get a reference to the application's user manager
            var userManager = Request.GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            // Go through the users, and extract their names
            foreach (var user in userManager.Users)
            {
                userList.Add(user.UserName);
            }

            return Ok(userList);
        }

        // Attention 34 - These methods enable testing of specific role claims - read carefully

        // The remaining tests will return...
        // if successfully authorized - a simple string message
        // if authorization fails - HTTP 401

        // Note the URI pattern for these tests
        // They aren't 'real world' or 'best practice', but they're simple

        // Any account
        [Authorize]
        [Route("api/test/anyaccount")]
        public IEnumerable<string> GetAnyAccount()
        {
            return new string[] { "any account", "works correctly" };
        }

        // Role "User"
        [Authorize(Roles = "User")]
        [Route("api/test/role/user")]
        public IEnumerable<string> GetRoleUser()
        {
            return new string[] { "role user", "works correctly" };
        }

        // Role "Student"
        [Authorize(Roles = "Student")]
        [Route("api/test/role/student")]
        public IEnumerable<string> GetRolestudent()
        {
            return new string[] { "role student", "works correctly" };
        }

        // Role "Dev"
        [Authorize(Users = "dev@example.com")]
        [Route("api/test/role/dev")]
        public IEnumerable<string> GetRoleDev()
        {
            return new string[] { "role dev", "works correctly" };
        }

    }

}
