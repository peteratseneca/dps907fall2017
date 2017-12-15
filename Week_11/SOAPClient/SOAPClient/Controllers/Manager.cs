using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// added...
using SOAPClient.SenecaES;

namespace SOAPClient.Controllers
{
    public class Manager
    {
        // Attention 01 - Reference to the web service proxy class, which was created with "Add Service Reference"
        EmployeeServiceClient es = new EmployeeServiceClient();

        // All employees
        public IEnumerable<EmployeeBase> EmployeeGetAll()
        {
            // Attention 02 - Call the web service method
            var fetchedObjects = es.AllEmployees();

            if (fetchedObjects == null)
            {
                return new List<EmployeeBase>();
            }
            else
            {
                return fetchedObjects;
            }
        }

        // Employee by identifier
        public EmployeeBase EmployeeGetById(int id)
        {
            // Call the web service method
            var fetchedObject = es.EmployeeById(id);

            return (fetchedObject == null) ? null : fetchedObject;
        }

        public EmployeeBase EmployeeAddNew(EmployeeAdd newItem)
        {
            // Call the web service method
            var addedItem = es.AddEmployee(newItem);

            return (addedItem == null) ? null : addedItem;
        }

    }

}
