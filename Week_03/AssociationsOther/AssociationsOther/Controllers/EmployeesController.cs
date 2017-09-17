using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace AssociationsOther.Controllers
{
    public class EmployeesController : ApiController
    {
        // Manager reference
        Manager m = new Manager();

        // GET: api/Employees
        public IHttpActionResult Get()
        {
            return Ok(m.EmployeeGetAll());
        }

        // The following "get one" method will return an object
        // with self associated (employee) data

        // You could write other methods, to return the bare employee object,
        // or to return an object with all associated data (including
        // address and job duty data) 
        // How? Simply write another manager method (already done)

        // GET: api/Employees/5
        public IHttpActionResult Get(int? id)
        {
            // Determine whether we can continue
            if (!id.HasValue) { return NotFound(); }

            // Fetch the object, so that we can inspect its value
            var o = m.EmployeeGetByIdWithAllInfo(id.Value);

            if (o == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(o);
            }
        }

        // POST: api/Employees
        public IHttpActionResult Post([FromBody]EmployeeAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null)
            {
                return BadRequest("Invalid request URI");
            }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            // Ensure that we can use the incoming data
            if (ModelState.IsValid)
            {
                // Attempt to add the new object
                var addedItem = m.EmployeeAdd(newItem);

                // Notice the ApiController convenience methods
                if (addedItem == null)
                {
                    // HTTP 400
                    return BadRequest("Cannot add the object");
                }
                else
                {
                    // HTTP 201 with the new object in the entity body
                    // Notice how to create the URI for the Location header

                    var uri = Url.Link("DefaultApi", new { id = addedItem.Id });
                    return Created<EmployeeBase>(uri, addedItem);
                }
            }
            else
            {
                // HTTP 400
                return BadRequest(ModelState);
            }
        }

        // PUT: api/Employees/5
        public IHttpActionResult Put(int id, [FromBody]EmployeeEditNames editedItem)
        {
            // Ensure that an "editedItem" is in the entity body
            if (editedItem == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id != editedItem.Id)
            {
                return BadRequest("Invalid data in the entity body");
            }

            // Ensure that we can use the incoming data
            if (ModelState.IsValid)
            {
                // Attempt to update the item
                var changedItem = m.EmployeeEditNames(editedItem);

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

        // Attention 40 - Command pattern for the employee entity, notice the void return type

        // We need to be able to set or clear an employee's supervisor (ReportsToEmployee)
        // In this design, the identifiers for both the employee and supervisor
        // are in the entity body (the resource model)

        // PUT: api/Employees/5/SetSupervisor
        [Route("api/employees/{id}/setsupervisor")]
        public void PutSetSupervisor(int id, [FromBody]EmployeeSupervisor item)
        {
            // Ensure that an "editedItem" is in the entity body
            if (item == null) { return; }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id != item.Employee) { return; }

            // Ensure that we can use the incoming data
            if (ModelState.IsValid)
            {
                // Attempt to update the item
                m.SetEmployeeSupervisor(item);
            }
            else
            {
                return;
            }
        }

        // PUT: api/Employees/5/ClearSupervisor
        [Route("api/employees/{id}/clearsupervisor")]
        public void PutClearSupervisor(int id, [FromBody]EmployeeSupervisor item)
        {
            // Ensure that an "editedItem" is in the entity body
            if (item == null) { return; }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id != item.Employee) { return; }

            // Ensure that we can use the incoming data
            if (ModelState.IsValid)
            {
                // Attempt to update the item
                m.ClearEmployeeSupervisor(item);
            }
            else
            {
                return;
            }
        }

        // PUT: api/Employees/5/SetJobDuty
        [Route("api/employees/{id}/setjobduty")]
        public void PutSetJobDuty(int id, [FromBody]EmployeeJobDuty item)
        {
            // Ensure that an "editedItem" is in the entity body
            if (item == null) { return; }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id != item.Employee) { return; }

            // Ensure that we can use the incoming data
            if (ModelState.IsValid)
            {
                // Attempt to update the item
                m.SetEmployeeJobDuty(item);
            }
            else
            {
                return;
            }
        }

        // PUT: api/Employees/5/ClearJobDuty
        [Route("api/employees/{id}/clearjobduty")]
        public void PutClearJobDuty(int id, [FromBody]EmployeeJobDuty item)
        {
            // Ensure that an "editedItem" is in the entity body
            if (item == null) { return; }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id != item.Employee) { return; }

            // Ensure that we can use the incoming data
            if (ModelState.IsValid)
            {
                // Attempt to update the item
                m.ClearEmployeeJobDuty(item);
            }
            else
            {
                return;
            }
        }

        // DELETE: api/Employees/5
        public void Delete(int id)
        {
            // In a controller 'Delete' method, a void return type will
            // automatically generate a HTTP 204 "No content" response
            m.EmployeeDelete(id);
        }

    }

}
