using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AssociationsIntro.Controllers
{
    public class CustomersController : ApiController
    {
        // Reference to the manager object
        private Manager m = new Manager();

        // Return types are IHttpActionResult for all except the Delete() method, which  void

        // GET: api/Customers
        public IHttpActionResult Get()
        {
            return Ok(m.CustomerGetAll());
        }

        // GET: api/Customers/5
        public IHttpActionResult Get(int? id)
        {
            // Attempt to fetch the object
            var o = m.CustomerGetById(id.GetValueOrDefault());

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

        // Get one, with associated object, use attribute routing

        // GET: api/Customers/5/WithEmployee
        [Route("api/customers/{id}/withemployee")]
        public IHttpActionResult GetWithEmployee(int? id)
        {
            // Attempt to fetch the object
            var o = m.CustomerGetByIdWithEmployee(id.GetValueOrDefault());

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

        // POST: api/Customers
        public IHttpActionResult Post([FromBody]CustomerAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null) { return BadRequest("Invalid request URI"); }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null) { return BadRequest("Must send an entity body with the request"); }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Attempt to add the new object
            var addedItem = m.CustomerAdd(newItem);

            // Continue?
            if (addedItem == null) { return BadRequest("Cannot add the object"); }

            // HTTP 201 with the new object in the entity body
            // Notice how to create the URI for the Location header
            var uri = Url.Link("DefaultApi", new { id = addedItem.CustomerId });

            return Created(uri, addedItem);
        }

        /*
        // PUT: api/Customers/5
        public void Put(int id, [FromBody]string value)
        {
        }
        */

        // DELETE: api/Customers/5
        public void Delete(int id)
        {
            // In a controller 'Delete' method, a void return type will
            // automatically generate a HTTP 204 "No content" response
            m.CustomerDelete(id);
        }
    }
}
