using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Web.Http;
using System.Net;

// HTTP OPTIONS handler

// Shout out to Jef Claes for the inspiration and design
// http://www.jefclaes.be/2012/09/supporting-options-verb-in-aspnet-web.html

// Students in Prof. McIntyre's Web Services course have permission to use this code as-is

// Add the following to the Register method body in the WebApiConfig class
//config.MessageHandlers.Add(new ServiceLayer.HandleHttpOptions());

namespace SecuredCustomer.ServiceLayer
{
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
}
