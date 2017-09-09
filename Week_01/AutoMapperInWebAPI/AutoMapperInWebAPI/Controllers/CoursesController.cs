using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AutoMapperInWebAPI.Controllers
{
    public class CoursesController : ApiController
    {
        // Attention 05 - Typical controller configuration...

        // Reference to the data manager
        // All methods return IHttpActionResult or void
        // Method construction is similar to what you know from ASP.NET MVC web apps

        // Reference to the data manager
        private Manager m = new Manager();

        // GET: api/Courses
        public IHttpActionResult Get()
        {
            return Ok(m.CourseGetAll());
        }

        // GET: api/Courses/5
        public IHttpActionResult Get(int? id)
        {
            // Attempt to locate the matching object
            var o = m.CourseGetOne(id.GetValueOrDefault());

            if (o == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(o);
            }
        }

        // POST: api/Courses
        public IHttpActionResult Post([FromBody]CourseAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null) { return BadRequest("Invalid request URI"); }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null) { return BadRequest("Must send an entity body with the request"); }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Attempt to add the new item...
            var addedItem = m.CourseAdd(newItem);

            // Continue?
            if (addedItem == null) { return BadRequest("Cannot add the object"); }

            // Return HTTP 201 with the new object in the message (entity) body
            // Notice how to create the URI for the required "Location" header
            var uri = Url.Link("DefaultApi", new { id = addedItem.Id });

            return Created(uri, addedItem);
        }

    }
}
