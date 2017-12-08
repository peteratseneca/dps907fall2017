using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ObjectOwner.Controllers
{
    [Authorize]
    public class NotesController : ApiController
    {
        // Reference to the manager
        private Manager m = new Manager();

        // GET: api/Notes
        public IHttpActionResult Get()
        {
            return Ok(m.NoteGetAll());
        }

        // GET: api/Notes/5
        public IHttpActionResult Get(int? id)
        {
            // Attempt to fetch the object
            var o = m.NoteGetById(id.GetValueOrDefault());

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

        // POST: api/Notes
        public IHttpActionResult Post([FromBody]NoteAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null) { return BadRequest("Invalid request URI"); }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null) { return BadRequest("Must send an entity body with the request"); }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Attempt to add the new object
            var addedItem = m.NoteAdd(newItem);

            // Continue?
            if (addedItem == null) { return BadRequest("Cannot add the object"); }

            // HTTP 201 with the new object in the entity body
            // Notice how to create the URI for the Location header
            var uri = Url.Link("DefaultApi", new { id = addedItem.Id});

            return Created(uri, addedItem);
        }

        /*
        // PUT: api/Notes/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Notes/5
        public void Delete(int id)
        {
        }
        */
    }
}
