using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using Newtonsoft.Json;

namespace SecuredCustomer.Controllers
{
    // This source code file has classes for link relations

    // There is a 'Link' class that models a hypermedia link 

    // There are two abstract classes 
    // One is for a single linked item 
    // The other is for a linked collection

    // A resource model class can inherit from one of these classes
    // The biggest benefit is the reduction in code-writing
    // For example, the LinkedItem<T> abstract class can be used as the 
    // base class for a 'Product' linked item, or for a 'Supplier' linked item
    // Study the source code for a resource model class cluster to see how it's used

    /// <summary>
    /// A hypermedia link
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Rel - relation
        /// </summary>
        public string Rel { get; set; }
        /// <summary>
        /// Href - hypermedia reference
        /// </summary>
        public string Href { get; set; }

        // New added properties...

        // The null value handling issue is controversial
        // Attributes were used here to make the result look nicer (without null-valued properties)
        // However, read these...
        // StackOverflow - http://stackoverflow.com/questions/10150312/removing-null-properties-from-json-in-mvc-web-api-4-beta
        // CodePlex - http://aspnetwebstack.codeplex.com/workitem/243

        /// <summary>
        /// ContentType - internet media type, for content negotiation
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ContentType { get; set; }

        /// <summary>
        /// Method - HTTP method(s) which can be used
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Method { get; set; }

        /// <summary>
        /// Title - human-readable title label
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
    }

    // Factory to create a linked ITEM

    // The abstract base classes for "linked" objects have been refactored
    // Each is a "factory" - it constructs an object that includes all the links
    // The benefit is code reduction - the resource model classes are smaller

    /// <summary>
    /// Encloses an 'item' in a media type that has a 'Links' collection
    /// Abstract base class, which must be inherited
    /// </summary>
    /// <typeparam name="T">View model object</typeparam>
    public abstract class LinkedItem<T>
    {
        public LinkedItem(T item)
        {
            Links = new List<Link>();

            Item = item;

            // Get the current request URL
            var absolutePath = HttpContext.Current.Request.Url.AbsolutePath;

            // Use the API Explorer service
            string[] u = absolutePath.Split(new char[] { '/' });
            // u[0] = (empty)
            // u[1] = "api"
            // u[2] = controller name
            // u[3] = id value
            var itemMethods = string.Join(",", ServiceLayer.ApiExplorerService.GetSupportedMethods(u[2], u[3]));
            var controllerMethods = string.Join(",", ServiceLayer.ApiExplorerService.GetSupportedMethods(u[2], null));

            // Link relation for 'self' in the item
            // Use "dynamic" to avoid having to create an interface
            // Using "dynamic" forces the programmer to ensure that the 
            // passed-in object does indeed include a property named "Link"
            dynamic i = Item;
            i.Link = new Link() { Rel = "self", Href = absolutePath, Method = itemMethods };

            // Link relation for 'self'
            this.Links.Add(new Link() { Rel = "self", Href = absolutePath, Method = itemMethods });

            // Link relation for 'collection'
            this.Links.Add(new Link() { Rel = "collection", Href = string.Format("/{0}/{1}", u[1], u[2]), Method = controllerMethods });
        }

        public LinkedItem(T item, int id)
        {
            // This constructor handles the "add new" use case
            // In an "add new" situation, the AbsolutePath is the collection
            // We need to add the new unique identifier on to the end

            Links = new List<Link>();

            Item = item;

            // Get the current request URL
            var absolutePath = HttpContext.Current.Request.Url.AbsolutePath;

            // Use "dynamic" to avoid having to create an interface
            // Using "dynamic" forces the programmer to ensure that the 
            // passed-in object does indeed include a property named "Link"
            dynamic i = Item;

            // Use the API Explorer service
            string[] u = absolutePath.Split(new char[] { '/' });
            // u[0] = (empty)
            // u[1] = "api"
            // u[2] = controller name
            // u[3] = id value
            var itemMethods = string.Join(",", ServiceLayer.ApiExplorerService.GetSupportedMethods(u[2], i.Id.ToString()));
            var controllerMethods = string.Join(",", ServiceLayer.ApiExplorerService.GetSupportedMethods(u[2], null));

            // Link relation for 'self' in the item
            // Add the unique identifier to the end of the absolutePath
            absolutePath += string.Format("/{0}", i.Id);
            i.Link = new Link() { Rel = "self", Href = absolutePath, Method = itemMethods };

            // Link relation for 'self'
            this.Links.Add(new Link() { Rel = "self", Href = absolutePath, Method = itemMethods });

            // Link relation for 'collection'
            this.Links.Add(new Link() { Rel = "collection", Href = string.Format("/{0}/{1}", u[1], u[2]), Method = controllerMethods });
        }

        /// <summary>
        /// Links for this item
        /// </summary>
        public List<Link> Links { get; set; }
        /// <summary>
        /// Data item
        /// </summary>
        public T Item { get; set; }
    }

    // Factory to create a linked COLLECTION

    /// <summary>
    /// Encloses a 'collection' in a media type that has a 'Links' collection
    /// Abstract base class, which must be inherited
    /// </summary>
    /// <typeparam name="T">View model collection</typeparam>
    public abstract class LinkedCollection<T>
    {
        public LinkedCollection(IEnumerable<T> collection)
        {
            this.Links = new List<Link>();

            Collection = collection;

            // Get the current request URL
            var absolutePath = HttpContext.Current.Request.Url.AbsolutePath;

            // Use the API Explorer service
            string[] u = absolutePath.Split(new char[] { '/' });
            // u[0] = (empty)
            // u[1] = "api"
            // u[2] = controller name
            // u[3] = id value
            var controllerMethods = string.Join(",", ServiceLayer.ApiExplorerService.GetSupportedMethods(u[2], null));

            // Link relation for 'self'
            this.Links.Add(new Link() { Rel = "self", Href = absolutePath, Method = controllerMethods });

            // Add 'item' links for each item in the collection
            // Use "dynamic" to avoid having to create an interface
            // Using "dynamic" forces the programmer to ensure that the 
            // object does indeed include a property named "Link"
            foreach (dynamic item in this.Collection)
            {
                // Use the API Explorer service
                var itemMethods = string.Join(",", ServiceLayer.ApiExplorerService.GetSupportedMethods(u[2], item.Id.ToString()));

                item.Link = new Link() { Rel = "item", Href = string.Format("{0}/{1}", absolutePath, item.Id), Method = itemMethods };
            }
        }

        /// <summary>
        /// Links for this collection
        /// </summary>
        public List<Link> Links { get; set; }
        /// <summary>
        /// Data collection
        /// </summary>
        public IEnumerable<T> Collection { get; set; }
    }

}
