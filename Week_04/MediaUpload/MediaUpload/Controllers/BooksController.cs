using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MediaUpload.Controllers
{
    public class BooksController : ApiController
    {
        // Reference to the data manager
        private Manager m = new Manager();

        // GET: api/Books
        public IHttpActionResult Get()
        {
            var c = m.BookGetAll();

            return Ok(c);
        }

        // GET: api/Books/5
        public IHttpActionResult Get(int? id)
        {
            // Attempt to fetch the object
            var o = m.BookGetById(id.GetValueOrDefault());

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

        // GET: api/Books/5/Photo
        [Route("api/books/{id}/photo")]
        public IHttpActionResult GetPhoto(int? id)
        {
            // Attention 15 - This method does NOT use content negotiation (aka conneg)
            // It uses the URI and an Accept header, and delivers only the photo bytes

            // Fetch the object
            var o = m.BookGetByIdWithMedia(id.GetValueOrDefault());

            // Continue?
            if (o == null | o.PhotoLength == 0) { return NotFound(); }

            // Attention 16 - Coding safety, when returning the media item

            // The following simple "return Ok(o.Photo)" statement will work, 
            // *if* the requestor includes the header "Accept: image/*"
            // Otherwise, it will send the photo bytes through the JSON serializer
            // That's not a good thing - we do not want that behaviour - it's unsafe
            // So, do NOT do the following:

            //return Ok(o.Photo);

            // A safer alternative is to do the following...

            // Get a reference to the media formatter that handles the content type
            var formatter = GlobalConfiguration.Configuration.Formatters.FindWriter(typeof(byte[]), new System.Net.Http.Headers.MediaTypeHeaderValue(o.ContentType));

            // Return the result, ensuring that it is processed by the media formatter
            return Content(HttpStatusCode.OK, o.Photo, formatter, o.ContentType);
        }

        // POST: api/Books
        public IHttpActionResult Post([FromBody]BookAdd newItem)
        {
            // Ensure that the URI is clean (and does not have an id parameter)
            if (Request.GetRouteData().Values["id"] != null) { return BadRequest("Invalid request URI"); }

            // Ensure that a "newItem" is in the entity body
            if (newItem == null) { return BadRequest("Must send an entity body with the request"); }

            // Ensure that we can use the incoming data
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Attempt to add the new object
            var addedItem = m.BookAdd(newItem);

            // Continue?
            if (addedItem == null) { return BadRequest("Cannot add the object"); }

            // HTTP 201 with the new object in the entity body
            // Notice how to create the URI for the Location header
            var uri = Url.Link("DefaultApi", new { id = addedItem.Id });

            return Created(uri, addedItem);
        }

        // PUT: api/Books/5/setphoto
        // Notice the use of the [HttpPut] attribute, which is an alternative to the method name starting with "Put..."
        [Route("api/books/{id}/setphoto")]
        [HttpPut]
        public IHttpActionResult BookPhoto(int id, [FromBody]byte[] photo)
        {
            // Get the Content-Type header from the request
            var contentType = Request.Content.Headers.ContentType.MediaType;

            // Attempt to save
            if (m.BookSetPhoto(id, contentType, photo))
            {
                // By convention, we have decided to return HTTP 204
                // It's a 'success' code, but there's no content for a 'command' task
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                // Uh oh, some error happened, so tell the requestor
                return BadRequest("Unable to set the photo");
            }
        }

        /*
        // PUT: api/Books/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Books/5
        public void Delete(int id)
        {
        }
        */
    }
}
