using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
// added in this version...
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;

namespace [your-project-name].ServiceLayer
{
    // Custom media formatter

    public class HRFormatterICT : JsonMediaTypeFormatter
    {
        public HRFormatterICT()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json+ict"));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        private int GetPattern()
        {
            // How many segments?
            var absolutePath = HttpContext.Current.Request.Url.Segments;

            // How many items were returned
            // Find out if the data is an object or collection, and the count

            // Is there any query string pairs
            var query = HttpContext.Current.Request.QueryString;
            // doesn't blow up if there's no query string
            var queryCount = query.Count;
            // this value is zero if there's no query string

            // From the routing infrastructure, is there an id value present
            HttpRequestMessage hrm = HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
            var routeData = hrm.GetRouteData();
            // yes, it works
            // has route template as a string
            // also has/shows "id" as a parameter

            return 0;
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, System.Text.Encoding content)
        {
            using (var writer = new StreamWriter(writeStream))
            {
                var pkg = new ICTMediaType();
                if (value != null)
                {
                    // c - collection...
                    var isCollection =
                        value.GetType().GetInterfaces()
                        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));


                    // tests...

                    var query = HttpContext.Current.Request.QueryString;
                    // doesn't blow up if there's no query string
                    var queryCount = query.Count;
                    // this value is zero if there's no query string


                    // tests...

                    var segments = HttpContext.Current.Request.Url.Segments;
                    HttpRequestMessage hrm = HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                    var routeData = hrm.GetRouteData();

                    // tests...

                    var absolutePath = HttpContext.Current.Request.Url.AbsolutePath;
                    string[] u = absolutePath.Split(new char[] { '/' });

                    if (isCollection)
                    {
                        IEnumerable collection = (IEnumerable)value;
                        int count = 0;
                        foreach (var item in collection)
                        {
                            count++;
                            IDictionary<string, object> newItem = new ExpandoObject();

                            // Go through the all the properties in an item
                            foreach (PropertyInfo prop in item.GetType().GetProperties())
                            {
                                // Checks to reject any property named "Id"
                                // TODO: Need to check if there actually exists any other Id properties!
                                if (!(prop.Name == "Id"))
                                {
                                    newItem.Add(prop.Name, prop.GetValue(item));
                                }

                                /* Creates "Id" property for the newitem
                                   The first word is extracted from the class name of the collection's object
                                   and used to identify the proper Id property.
                                */

                                var objProp = value.GetType().GetProperties();
                                string objName = objProp[2].PropertyType.Name;
                                int index = 0;
                                int upperCaseCount = 0;
                                foreach (Char c in objName)
                                {
                                    index++;
                                    if (Char.IsUpper(c))
                                        upperCaseCount++;
                                    if (upperCaseCount > 1)
                                    {
                                        break;
                                    }
                                }
                                objName = objName.Remove(index - 1);
                                objName = objName + "Id";
                                if (prop.Name == objName)
                                {
                                    newItem.Add("Id", prop.GetValue(item));
                                }
                            }

                            // Add the links (below)
                            dynamic o = item;

                            //var itemMethods = string.Join(",", ApiExplorerService.GetSupportedMethods(u[2], o.Id.ToString()));
                            var itemMethods = string.Join(",", ApiExplorerService.GetSupportedMethods(u[2], "123"));
                            //newItem.Add("Link", new link() { rel = "item", href = string.Format("{0}/{1}", absolutePath, o.Id), methods = itemMethods });
                            newItem.Add("Link", new link() { rel = "item", href = string.Format("{0}/{1}", absolutePath, 123), methods = itemMethods });
                            pkg.data.Add(newItem);
                        }
                        var controllerMethods = string.Join(",", ApiExplorerService.GetSupportedMethods(u[2], null));

                        // Link relation for 'self'
                        pkg.links.Add(new link() { rel = "self", href = absolutePath, methods = controllerMethods });

                        pkg.count = count;
                    }
                    else
                    {
                        IDictionary<string, object> newItem = new ExpandoObject();

                        // Go through the all the properties in an item
                        foreach (PropertyInfo prop in value.GetType().GetProperties())
                        {
                            newItem.Add(prop.Name, prop.GetValue(value));
                        }

                        var itemMethods = string.Join(",", ApiExplorerService.GetSupportedMethods(u[2], u[3]));
                        newItem.Add("Link", new link() { rel = "self", href = absolutePath, methods = itemMethods });

                        // Link relation for 'self'
                        pkg.links.Add(new link() { rel = "self", href = absolutePath, methods = itemMethods });

                        var controllerMethods = string.Join(",", ApiExplorerService.GetSupportedMethods(u[2], null));

                        // Link relation for 'collection'
                        pkg.links.Add(new link() { rel = "collection", href = string.Format("/{0}", u[1]), methods = controllerMethods });

                        pkg.count = 1;
                        pkg.data.Add(newItem);
                    }
                }
                string json = JsonConvert.SerializeObject(pkg, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                var buffer = Encoding.Default.GetBytes(json);
                writeStream.Write(buffer, 0, buffer.Length);
                writeStream.Flush();
                writeStream.Close();
            }
        }
    }


    // API explorer service, programmatically inspects a controller
    // to get information on what HTTP methods are supported by its methods

    // Shout out to Jef Claes for the inspiration and design
    // http://www.jefclaes.be/2012/09/supporting-options-verb-in-aspnet-web.html

    // Students in Prof. McIntyre's Web Services course have permission to use this code as-is

    public class ApiExplorerService
    {
        public static IEnumerable<string> GetSupportedMethods(string controllerRequested, string idRequested)
        {
            // Get a reference to the API Explorer
            var apiExplorer = GlobalConfiguration.Configuration.Services.GetApiExplorer();

            // Three possible situations for the URI path...
            // 1. Controller, no id -       /api/items
            // 2. Controller, id -          /api/items/3
            // 3. No controller, no id -    /api

            // Collector for the supported methods
            IEnumerable<string> supportedMethods = null;

            if (string.IsNullOrEmpty(idRequested))
            {
                // 1. Controller, no id -       /api/items
                // ##################################################

                supportedMethods = apiExplorer.ApiDescriptions.Where(d =>
                {
                    // In the controller class, look for methods that match
                    // the requested controller name and nothing for the id parameter
                    var controller = d.ActionDescriptor.ControllerDescriptor.ControllerName;
                    var idParameter = d.ParameterDescriptions.SingleOrDefault(p => p.Name == "id");
                    bool doesControllerMatch = string.Equals(controller, (string)controllerRequested, StringComparison.OrdinalIgnoreCase);
                    bool isIdNull = (idParameter == null) ? true : false;
                    return doesControllerMatch & isIdNull;
                })
                .Select(d => d.HttpMethod.Method)
                .Distinct();
            }
            else
            {
                // 2. Controller, id -          /api/items/3
                // ##################################################

                supportedMethods = apiExplorer.ApiDescriptions.Where(d =>
                {
                    // In the controller class, look for methods that match
                    // the requested controller name and the presence of an id parameter
                    var controller = d.ActionDescriptor.ControllerDescriptor.ControllerName;
                    var idParameter = d.ParameterDescriptions.SingleOrDefault(p => p.Name == "id");
                    bool doesControllerMatch = string.Equals(controller, (string)controllerRequested, StringComparison.OrdinalIgnoreCase);
                    bool hasId = (idParameter == null) ? false : true;
                    return doesControllerMatch & hasId;
                })
                .Select(d => d.HttpMethod.Method)
                .Distinct();
            }

            // 3. No controller, no id -    /api
            // ##################################################

            if (string.IsNullOrEmpty((string)controllerRequested))
            {
                supportedMethods = apiExplorer.ApiDescriptions.Where(d =>
                {
                    // In the RootController class, look for matching methods
                    var controller = d.ActionDescriptor.ControllerDescriptor.ControllerName;
                    return string.Equals(controller, "root", StringComparison.OrdinalIgnoreCase);
                })
                    .Select(d => d.HttpMethod.Method)
                    .Distinct();
            }

            return supportedMethods;
        }
    }

    // HTTP OPTIONS handler

    // Add the following to the Register method body in the WebApiConfig class
    //// Handle HTTP OPTIONS requests
    //config.MessageHandlers.Add(new ServiceLayer.HandleHttpOptions());

    public class HandleHttpOptions : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Options)
            {
                // Get the controller and id values
                var controllerRequested = request.GetRouteData().Values["controller"] as string;
                var idRequested = request.GetRouteData().Values["id"] as string;

                // Collector for the supported methods
                IEnumerable<string> supportedMethods = ApiExplorerService.GetSupportedMethods(controllerRequested, idRequested);

                // The controllerRequested does not exist, so return HTTP 404
                if (!supportedMethods.Any())
                {
                    return Task.Factory.StartNew(() => request.CreateResponse(HttpStatusCode.NotFound));
                }

                return Task.Factory.StartNew(() =>
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.OK);
                    string methods = string.Join(",", supportedMethods);
                    // For standard requests, add the 'Allow' header
                    resp.Content = new StringContent("");
                    resp.Content.Headers.Add("Allow", methods);
                    // For Ajax requests
                    resp.Headers.Add("Access-Control-Allow-Origin", "*");
                    resp.Headers.Add("Access-Control-Allow-Methods", methods);

                    return resp;
                });
            }

            return base.SendAsync(request, cancellationToken);
        }
    }

    // This is an example of a hypermedia representation
    // We will adapt this to its final form next week

    public class ICTMediaType
    {
        public ICTMediaType()
        {
            timestamp = DateTime.Now;
            count = 0;
            version = "1.0.0";
            data = new List<dynamic>();
            links = new List<link>();
        }
        public DateTime timestamp { get; set; }
        public string version { get; set; }
        public int count { get; set; }
        public ICollection<dynamic> data { get; set; }
        public List<link> links { get; set; }
    }

    // This is a "link" class that describes a link relation

    // All symbols are lower-case, to conform to web standards

    /// <summary>
    /// A hypermedia link
    /// </summary>
    public class link
    {
        public link()
        {
            //fields = new List<field>();
        }

        /// <summary>
        /// Relation kind
        /// </summary>
        public string rel { get; set; } = "";

        /// <summary>
        /// Hypermedia reference URL segment
        /// </summary>
        public string href { get; set; } = "";

        // New added properties...

        // The null value handling issue is controversial
        // Attributes were used here to make the result look nicer (without null-valued properties)
        // However, read these...
        // StackOverflow - http://stackoverflow.com/questions/10150312/removing-null-properties-from-json-in-mvc-web-api-4-beta
        // CodePlex - http://aspnetwebstack.codeplex.com/workitem/243

        /// <summary>
        /// Internet media type, for content negotiation
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }

        /// <summary>
        /// HTTP method(s) which can be used
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string methods { get; set; }

        /// <summary>
        /// Human-readable title label
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string title { get; set; }

        /// <summary>
        /// Values which must be sent with the request
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<field> fields { get; set; }
    }

    public class field
    {
        /// <summary>
        /// Name of the field
        /// </summary>
        public string name { get; set; } = "";

        /// <summary>
        /// Data type of the field
        /// </summary>
        public string type { get; set; } = "";

        /// <summary>
        /// Initial value of the field (if available)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string value { get; set; }
    }

}