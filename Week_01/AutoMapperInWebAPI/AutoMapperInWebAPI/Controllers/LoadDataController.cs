using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AutoMapperInWebAPI.Controllers
{
    public class LoadDataController : ApiController
    {
        // Attention 11 - This is a throw-away controller
        // We don't need it after some initial data gets created

        // Reference to the data manager
        private Manager m = new Manager();

        // GET: api/LoadData
        public string Get()
        {
            return m.LoadData() ? "New data has been loaded" : "Data already has been loaded";
        }

    }
}
