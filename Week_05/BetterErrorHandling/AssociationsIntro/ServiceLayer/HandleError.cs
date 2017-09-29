using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace AssociationsIntro.ServiceLayer
{
    // Attention 01 - This is the customized error handler

    // To initialize, add the following to the Register method in the WebApiConfig class
    //// Add HandleError to the pipeline
    //config.Services.Replace(typeof(IExceptionHandler), new ServiceLayer.HandleError());

    // Also, to that source code file, make sure you add this "using" directive
    //using System.Web.Http.ExceptionHandling;

    public class HandleError : ExceptionHandler
    {
        // Attention 02 - Custom packaging for the error info
        private class ErrorInfo
        {
            public string Message { get; set; }
            public DateTime Timestamp { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string StackTrace { get; set; }
        }

        // Attention 03 - In a class based on ExceptionHandler, implement the Handle() method
        public override void Handle(ExceptionHandlerContext context)
        {
            // Create a new ErrorInfo object
            var errorInfo = new ErrorInfo
            {
                Message = context.Exception.Message,
                Timestamp = DateTime.Now
            };

            if (context.Request.IsLocal())
            {
                // Add the stack trace, and show it to the programmer
                errorInfo.StackTrace = context.Exception.StackTrace;
            }

            // Add the error info to the response
            context.Result = new ResponseMessageResult
                (context.Request.CreateResponse
                (HttpStatusCode.InternalServerError, errorInfo));
        }

    }
}
