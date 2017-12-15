using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AssociationsOther.Controllers;
using System.ComponentModel.DataAnnotations;

namespace AssociationsOther
{
    // NOTE: In order to launch WCF Test Client for testing this service, 
    // please select EmployeeService.svc or EmployeeService.svc.cs in Solution Explorer and start debugging.

    // Attention 06 - This is the class that inherits from the interface
    
    // Attention 07 - After adding methods to the interface, the compiler will highlight an error in this class
    // It complains that it does not implement the methods
    // You can easily fix this by getting Visual Studio to create the stub method implementations
    // Ctrl+. and then choose "Implement interface"

    public class EmployeeService : IEmployeeService
    {
        // Attention 08 - Manager reference
        private Manager m = new Manager();

        public EmployeeBase AddEmployee(EmployeeAdd newItem)
        {
            // Attention 10 - Can copy most of the logic from the EmployeesController (Web API)

            // Ensure that a "newItem" is in the entity body
            if (newItem == null)
            {
                return null;
            }

            // Ensure that we can use the incoming data

            // Attention 11 - We do not have access to ModelState in this kind of class
            // However, we can add it in...

            var vc = new ValidationContext(newItem, null, null);
            var modelStateIsValid = Validator.TryValidateObject(newItem, vc, null, true);

            if (modelStateIsValid)
            {
                // Attempt to add the new object
                var addedItem = m.EmployeeAdd(newItem);

                // Notice the ApiController convenience methods
                if (addedItem == null)
                {
                    return null;
                }
                else
                {
                    // Return the new object in the entity body
                    return addedItem;
                }
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<EmployeeBase> AllEmployees()
        {
            return m.EmployeeGetAll();
        }

        public EmployeeBase3 EmployeeById(int? id)
        {
            // Attention 09 - Can copy most of the logic from the EmployeesController (Web API)

            // Determine whether we can continue
            if (!id.HasValue) { return null; }

            // Fetch the object, so that we can inspect its value
            var o = m.EmployeeGetByIdWithAllInfo(id.Value);

            if (o == null)
            {
                return null;
            }
            else
            {
                return o;
            }
        }

        public string HelloWorld()
        {
            return "Hello, world!";
        }
    }
}
