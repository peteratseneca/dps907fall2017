using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
// added...
using System.ComponentModel.DataAnnotations;

namespace SimpleWebService.Controllers
{
    // Scroll down to the end of this source code file
    // There is a model class for a Teacher object

    public class TeachersController : ApiController
    {
        private List<Teacher> Teachers = new List<Teacher>();

        // Constructor
        public TeachersController()
        {
            // The collection is an in-memory collection only
            // Its lifetime is short... it begins life when the controller is initialized,
            // and it ends its life when the called/executed method finishes/exits

            Teachers.Add(new Teacher { Id = 1, FamilyName = "Tipson", GivenNames = "Ian", BirthDate = new DateTime(1986, 2, 3) });
            Teachers.Add(new Teacher { Id = 2, FamilyName = "McIntyre", GivenNames = "Peter", BirthDate = new DateTime(1988, 4, 13) });
            Teachers.Add(new Teacher { Id = 3, FamilyName = "Kubba", GivenNames = "Nagham", BirthDate = new DateTime(1990, 6, 3) });
            Teachers.Add(new Teacher { Id = 4, FamilyName = "Paracha", GivenNames = "Asma", BirthDate = new DateTime(1992, 8, 13) });
            Teachers.Add(new Teacher { Id = 5, FamilyName = "Laurin", GivenNames = "Cindy", BirthDate = new DateTime(1994, 10, 3) });
        }

        // GET: api/Teachers
        // Notice the IHttpActionResult return type
        public IHttpActionResult Get()
        {
            // Get all, ordered
            var result = Teachers.OrderBy(t => t.FamilyName).ThenBy(t => t.GivenNames);

            return Ok(result);
        }

        // GET: api/Teachers/5
        // Notice the nullable int argument, for safety
        public IHttpActionResult Get(int? id)
        {
            // Attempt to locate the matching object
            var result = Teachers.SingleOrDefault(t => t.Id == id.GetValueOrDefault());

            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        // POST: api/Teachers
        // Notice the use of a custom resource (view) model class
        public IHttpActionResult Post([FromBody]TeacherAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null) { return BadRequest("Invalid request URI"); }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null) { return BadRequest("Must send an entity body with the request"); }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Attempt to add the new item...

            // Generate the identifier
            var newid = Teachers.Select(t => t.Id).Max() + 1;

            // Add to the collection
            Teachers.Add(new Teacher
            {
                Id = newid,
                FamilyName = newItem.FamilyName,
                GivenNames = newItem.GivenNames,
                BirthDate = newItem.BirthDate
            });

            // Return the result
            var result = Teachers.SingleOrDefault(t => t.Id == newid);

            // HTTP 201 with the new object in the entity body
            // Notice how to create the URI for the Location header
            var uri = Url.Link("DefaultApi", new { id = result.Id });

            return Created(uri, result);
        }

        /*
        // PUT: api/Teachers/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Teachers/5
        public void Delete(int id)
        {
        }
        */
    }

    // Resource (view) model classes

    public class Teacher
    {
        public Teacher()
        {
            BirthDate = DateTime.Now.AddYears(-30);
        }

        [Range(1, Int16.MaxValue)]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string FamilyName { get; set; }

        [Required, StringLength(50)]
        public string GivenNames { get; set; }

        public DateTime BirthDate { get; set; }
    }

    public class TeacherAdd
    {
        public TeacherAdd()
        {
            BirthDate = DateTime.Now.AddYears(-30);
        }

        [Required, StringLength(50)]
        public string FamilyName { get; set; }

        [Required, StringLength(50)]
        public string GivenNames { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
