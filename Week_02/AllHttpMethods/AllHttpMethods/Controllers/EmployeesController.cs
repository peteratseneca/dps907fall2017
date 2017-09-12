using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AllHttpMethods.Controllers
{
    public class EmployeesController : ApiController
    {
        // Reference to the data service operations manager class
        private Manager m = new Manager();

        // Attention 04 - Get all
        // GET: api/Employees
        public IHttpActionResult Get()
        {
            return Ok(m.EmployeeGetAll());
        }

        // Attention 05 - Get one by its identifier
        // GET: api/Employees/5
        public IHttpActionResult Get(int? id)
        {
            // Attempt to fetch the object
            var o = m.EmployeeGetById(id.GetValueOrDefault());

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

        // Attention 06 - Add new
        // POST: api/Employees
        public IHttpActionResult Post([FromBody]EmployeeAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null) { return BadRequest("Invalid request URI"); }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null) { return BadRequest("Must send an entity body with the request"); }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Attempt to add the new object
            var addedItem = m.EmployeeAdd(newItem);

            // Continue?
            if (addedItem == null) { return BadRequest("Cannot add the object"); }

            // HTTP 201 with the new object in the entity body
            // Notice how to create the URI for the Location header
            var uri = Url.Link("DefaultApi", new { id = addedItem.EmployeeId });

            return Created(uri, addedItem);
        }

        // Attention 07 - Edit existing
        // PUT: api/Employees/5
        public IHttpActionResult Put(int? id, [FromBody]EmployeeEditContactInfo editedItem)
        {
            // Ensure that an "editedItem" is in the entity body
            if (editedItem == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id.GetValueOrDefault() != editedItem.EmployeeId)
            {
                return BadRequest("Invalid data in the entity body");
            }

            // Ensure that we can use the incoming data
            if (ModelState.IsValid)
            {
                // Attempt to update the item
                var changedItem = m.EmployeeEditContactInfo(editedItem);

                // Notice the ApiController convenience methods
                if (changedItem == null)
                {
                    // HTTP 400
                    return BadRequest("Cannot edit the object");
                }
                else
                {
                    // HTTP 200 with the changed item in the entity body
                    return Ok<EmployeeBase>(changedItem);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Attention 08 - Delete item
        // DELETE: api/Employees/5
        public void Delete(int id)
        {
            // In a controller 'Delete' method, a void return type will
            // automatically generate a HTTP 204 "No content" response
            m.EmployeeDelete(id);
        }
    }
}
