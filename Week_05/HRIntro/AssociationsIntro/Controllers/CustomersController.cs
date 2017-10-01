using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
// added...
using AssociationsIntro.ServiceLayer;

namespace AssociationsIntro.Controllers
{
    /// <summary>
    /// Actions for customers
    /// </summary>
    public class CustomersController : ApiController
    {
        // Reference to the manager object
        private Manager m = new Manager();

        // Return types are IHttpActionResult for all except the Delete() method, which  void

        // GET: api/Customers
        /// <summary>
        /// All customers
        /// </summary>
        /// <returns>Collection of Customer objects, sorted</returns>
        public IHttpActionResult Get()
        {
            var c = m.CustomerGetAll();

            // Attention 06 - Original return value
            //return Ok(c);

            // Attention 07 - New return value, with link relations

            // First, transform the object graph into one that includes the link property
            var lc = m.mapper.Map<IEnumerable<CustomerBaseWithLinks>>(c);

            // Next, create a hypermedia representation package
            var hr = new MediaTypeExample();

            // Go through each item in the collection, and configure link relations
            foreach (var item in lc)
            {
                // Add a link to "self" (il is "item link")
                var il = new link();
                il.rel = "self";
                il.href = $"/api/customers/{item.CustomerId}";
                item.links.Add(il);

                // Add a link to "collection" (cl is "collection link)
                var cl = new link();
                cl.rel = "collection";
                cl.href = "/api/customers";
                item.links.Add(cl);

                // Add this updated item to the hypermedia representation package
                hr.data.Add(item);
            }

            // Finish off the configuration of the hypermedia representation...

            // How many items?
            hr.count = hr.data.Count;

            // Configure the link for the collection to refer to itself
            var l = new link();
            l.rel = "self";
            l.href = "/api/customers";

            // Add it
            hr.links.Add(l);

            // Return the result
            return Ok(hr);
        }

        // GET: api/Customers/5
        /// <summary>
        /// Specific customer, using its identifier
        /// </summary>
        /// <param name="id">Customer identifier</param>
        /// <returns>Customer object</returns>
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
                // Attention 11 - Original return value
                //return Ok(o);

                // Attention 12 - New return value

                // Make a version of the object that includes a link relation
                var lo = m.mapper.Map<CustomerBaseWithLinks>(o);

                // Configure the link relation
                var l = new link();
                l.rel = "self";
                l.href = $"/api/customers/{o.CustomerId}";
                lo.links.Add(l);

                // Create and configure a package (hypermedia representation)
                var hr = new MediaTypeExample();

                hr.data.Add(lo);
                hr.count = hr.data.Count;
                hr.links.Add(l);

                // Add a link to the parent collection
                var lc = new link();
                lc.rel = "collection";
                lc.href = "/api/customers";
                hr.links.Add(lc);

                return Ok(hr);
            }
        }

        // Get one, with associated object, use attribute routing

        // GET: api/Customers/5/WithEmployee
        /// <summary>
        /// Specific customer and associated employee data, using its identifier
        /// </summary>
        /// <param name="id">Customer identifier</param>
        /// <returns>Customer object</returns>
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
        /// <summary>
        /// Add new customer
        /// </summary>
        /// <param name="newItem">New customer object</param>
        /// <returns>Customer object</returns>
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
        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="id">Customer identifier</param>
        public void Delete(int id)
        {
            // In a controller 'Delete' method, a void return type will
            // automatically generate a HTTP 204 "No content" response
            m.CustomerDelete(id);
        }
    }
}
