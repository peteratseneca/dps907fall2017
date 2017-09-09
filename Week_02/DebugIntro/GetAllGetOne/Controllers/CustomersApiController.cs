using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

// Change the namespace to avoid class name collision
namespace GetAllGetOne.Controllers.Api
{
    public class CustomersController : ApiController
    {
        // Reference to a manager object
        private Manager m = new Manager();

        // GET: api/CustomersApi
        public IHttpActionResult Get()
        {
            // Attention 01 (web service) - Error - object reference not set to an instance of an object

            // Just as it says... an object is null, and you're trying to use it
            // Set a breakpoint near this line, then "step over" to get to the error
            // Inspect the variable values as you step through the lines of code

            // Create a new CustomerEditContactInfo object
            // Intentionally leave out some property values
            // To fix, simply un-comment out the Fax and Email lines of code below
            var newCust = new CustomerEditContactInfo
            {
                //Fax = "(416) 661-4034",
                //Email = "customer@example.com",
                CustomerId = 2,
                Phone = "(416) 491-5050"
            };

            // Now, attempt to use (call, reference) some values that are not set
            // To fix, un-comment the Fax and Email lines of code above
            var email = newCust.Email;
            var emailLength = email.Length;

            // End of error block; the remaining code will work as expected

            // Fetch the collection
            var c = m.CustomerGetAll();

            // Pass the collection to the view
            return Ok(c);
        }

        /*
        // GET: api/CustomersApi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CustomersApi
        public void Post([FromBody]string value)
        {
        }
        */

        // PUT: api/CustomersApi/5
        public IHttpActionResult Put(int id, [FromBody]CustomerEditContactInfo editedItem)
        {
            // Ensure that an "editedItem" is in the entity body
            if (editedItem == null)
            {
                return BadRequest("Must send an entity body with the request");
            }

            // Ensure that the id value in the URI matches the id value in the entity body
            if (id != editedItem.CustomerId)
            {
                return BadRequest("Invalid data in the entity body");
            }

            // Ensure that we can use the incoming data
            if (ModelState.IsValid)
            {
                // Attention 03 (web service) - Error - cause a validation error when saving to the store

                // At this point in the method, newItem has passed the initial validation test
                // Set a breakpoint near this line 
                // So, let's invalidate some of its values (null and string too long),
                // and pass it to the manager, which will attempt to save it to the data store
                // Then, BOOM!, a DbEntity validation error will appear

                // Set a required value to null
                // To fix this, comment out the following line
                editedItem.Email = null;

                // Set another to a too-long value
                // To fix this, comment out the following line
                editedItem.Phone = "This string is too long. It exceeds the 24-character limit.";

                // Attempt to do the update
                var changedItem = m.CustomerEditContactInfo(editedItem);

                if (changedItem == null)
                {
                    // Return HTTP 400
                    return BadRequest("Cannot edit the object");
                }
                else
                {
                    // Return HTTP 200 with the changed item in the entity body
                    return Ok<CustomerBase>(changedItem);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /*
        // DELETE: api/CustomersApi/5
        public void Delete(int id)
        {
        }
        */
    }
}
