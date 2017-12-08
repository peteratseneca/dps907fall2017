using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IA.Controllers
{
    [Authorize(Roles = "UserAccountManager")]
    public class AppClaimsController : ApiController
    {
        // Manager reference
        private Manager m = new Manager();

        // GET: api/AppClaims
        public IHttpActionResult Get()
        {
            return Ok(m.AppClaimGetAllActive());
        }

        // GET: api/AppClaims/5
        public IHttpActionResult Get(int? id)
        {
            // Attempt to fetch the object
            var o = m.AppClaimGetById(id.GetValueOrDefault());

            // Continue?
            if (o == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(o);
            }
        }

        // GET: api/AppClaims/Type/Foo
        [Route("api/appclaims/type/{claimType}")]
        public IHttpActionResult GetByType(string claimType = "")
        {
            // Attempt to fetch the collection
            var c = m.AppClaimGetByType(claimType.Trim().ToLower());

            return Ok(c);
        }

        // GET: api/AppClaims/Type/Foo/Value/Bar
        [Route("api/appclaims/type/{claimType}/value/{claimValue}")]
        public IHttpActionResult GetByMatch(string claimType = "", string claimValue = "")
        {
            // Attempt to fetch the object
            var o = m.AppClaimGetByMatch(claimType.Trim().ToLower(), claimValue.Trim().ToLower());

            if (o == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(o);
            }
        }

        // POST: api/AppClaims
        public IHttpActionResult Post([FromBody]AppClaimAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null) { return BadRequest("Invalid request URI"); }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null) { return BadRequest("Must send an entity body with the request"); }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Attempt to add the new object
            var addedItem = m.AppClaimAdd(newItem);

            // Continue?
            if (addedItem == null) { return BadRequest("Cannot add the object"); }

            // HTTP 201 with the new object in the entity body
            // Notice how to create the URI for the Location header
            var uri = Url.Link("DefaultApi", new { id = addedItem.Id });

            return Created(uri, addedItem);
        }

        // PUT: api/AppClaims/5
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }

        // DELETE: api/AppClaims/5
        public void Delete(int id)
        {
            throw new HttpResponseException(HttpStatusCode.NotImplemented);
        }
    }
}
