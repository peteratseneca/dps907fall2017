using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
// added...
using System.Security.Claims;

namespace SecuredCustomer.Controllers
{
    public class ValuesController : ApiController
    {
        // Reference to the data manager
        private Manager m = new Manager();

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        // Attention 07 - Any kind of Authorize or AuthorizeClaim attribute can be added to a controller
        [Authorize]
        public string Get(int id)
        {
            // Attention 08 - We can also query the security principal, and read its data

            var user = User as ClaimsPrincipal;

            return string.Format("The authenticated user is {0}", user.Identity.Name);
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
