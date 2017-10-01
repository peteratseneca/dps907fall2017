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
    public class RootController : ApiController
    {
        // Attention 12 - This controller answers a URI that ends with 'api'
        // It's intended to handle requests to the 'root' URI, 
        // and will return a collection of link relations

        // This controller is called because we added a default value for 'controller'
        // in the App_Start > WebApiConfig class
        // How? Add a new item to the "defaults" field...
        // defaults: new { controller = "Root", id = RouteParameter.Optional }
        //                 ---------------------

        // GET: api/Root (or "/api" or "/api/")
        public IHttpActionResult Get()
        {
            // Create a collection of Link objects

            List<link> links = new List<link>();
            links.Add(new link() { rel = "collection", href = "/api/customers", methods = "GET,POST" });
            links.Add(new link() { rel = "collection", href = "/api/employees", methods = "GET,POST" });
            links.Add(new link() { rel = "collection", href = "/api/foo", methods = "GET,POST" });
            links.Add(new link() { rel = "collection", href = "/api/bar", methods = "GET,POST" });
            links.Add(new link() { rel = "command", href = "/api/baz/{id}/task", methods = "PUT" });

            // Create and configure a dictionary to hold the collection
            // We need to return a simple object, so a Dictionary<TKey, TValue> is ideal
            // This serializes nicely to JSON

            Dictionary<string, List<link>> linkList = new Dictionary<string, List<link>>();
            linkList.Add("links", links);

            return Ok(linkList);
        }
    }
}
