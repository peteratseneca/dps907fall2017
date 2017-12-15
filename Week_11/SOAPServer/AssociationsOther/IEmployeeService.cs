using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AssociationsOther
{
    [ServiceContract]
    public interface IEmployeeService
    {
        // Attention 01 - This is the interface, which defines the service's behaviour
        
        // Attention 02 - The OperationContract attribute is required on all publicly-accessible methods
        [OperationContract]
        string HelloWorld();

        [OperationContract]
        IEnumerable<Controllers.EmployeeBase> AllEmployees();

        [OperationContract]
        Controllers.EmployeeBase3 EmployeeById(int? id);

        [OperationContract]
        Controllers.EmployeeBase AddEmployee(Controllers.EmployeeAdd newItem);
    }
}
