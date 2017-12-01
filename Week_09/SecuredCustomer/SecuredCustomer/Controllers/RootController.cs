using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SecuredCustomer.Controllers
{
    public class RootController : ApiController
    {
        // This controller answers a URI that ends with 'api'
        // It's intended to handle requests to the 'root' URI, 
        // and will return a collection of link relations

        // This controller is called because we added a default value for 'controller'
        // in the App_Start > WebApiConfig class

        // GET: api/Root (or "/api" or "/api/")
        public IHttpActionResult Get()
        {
            // Create a collection of Link objects

            List<Link> links = new List<Link>();
            links.Add(new Link() { Rel = "collection", Href = "/api/foo", Method = "GET,POST" });
            links.Add(new Link() { Rel = "collection", Href = "/api/bar", Method = "GET,POST" });
            links.Add(new Link() { Rel = "collection", Href = "/api/baz/{id}/task", Method = "PUT" });

            // Create and configure a dictionary to hold the collection
            // We need to return a simple object, so a Dictionary<TKey, TValue> is ideal

            Dictionary<string, List<Link>> linkList = new Dictionary<string, List<Link>>();
            linkList.Add("Links", links);

            return Ok(linkList);
        }
    }
}
