using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AssociationsIntro.Controllers
{
    public class ExampleController : ApiController
    {
        // Reference to the data manager
        private Manager m = new Manager();

        // GET: api/Example
        public IHttpActionResult Get()
        {
            return Ok(m.ExampleGetAll());
        }

        // GET: api/Example/5
        public IHttpActionResult Get(int? id)
        {
            // Attempt to fetch the object
            var o = m.ExampleGetById(id.GetValueOrDefault());

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

        // POST: api/Example
        public IHttpActionResult Post([FromBody]string newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null) { return BadRequest("Invalid request URI"); }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null) { return BadRequest("Must send an entity body with the request"); }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Attempt to add the new object
            var addedItem = m.ExampleAdd(newItem);

            // Continue?
            if (addedItem == null) { return BadRequest("Cannot add the object"); }

            // HTTP 201 with the new object in the entity body
            // Notice how to create the URI for the Location header
            var uri = Url.Link("DefaultApi", new { id = 123 });

            return Created(uri, addedItem);
        }

        // PUT: api/Example/5
        public IHttpActionResult Put(int? id, [FromBody]string editedItem)
        {
            // Ensure that an "editedItem" is in the entity body
            if (editedItem == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id.GetValueOrDefault() != 123)
            {
                return BadRequest("Invalid data in the entity body");
            }

            // Ensure that we can use the incoming data
            if (ModelState.IsValid)
            {
                // Attempt to update the item
                var changedItem = m.ExampleEditSomething(editedItem);

                // Notice the ApiController convenience methods
                if (changedItem == null)
                {
                    // HTTP 400
                    return BadRequest("Cannot edit the object");
                }
                else
                {
                    // HTTP 200 with the changed item in the entity body
                    return Ok<string>(changedItem);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Example/5
        public void Delete(int id)
        {
            // In a controller 'Delete' method, a void return type will
            // automatically generate a HTTP 204 "No content" response
            m.ExampleDelete(id);
        }

    }
}
