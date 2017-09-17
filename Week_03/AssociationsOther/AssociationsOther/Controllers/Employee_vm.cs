using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace AssociationsOther.Controllers
{
    // Attention 15 - Employee resource models

	public class EmployeeAdd
	{
        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }
    }

    public class EmployeeBase : EmployeeAdd
    {
        public int Id { get; set; }
    }

    public class EmployeeBase2 : EmployeeBase
    {
        // Attention 16 - Employee, with "reports to" and "employees supervised" info
        // This resource model class includes data for self associations,
        // including "reports to" and "employees supervised"
        // We DO NOT need all these properties; I simply wanted to show
        // what the alternatives were, so you could choose the best one
        // for your use case

        public EmployeeBase2()
        {
            EmployeesSupervised = new List<EmployeeBase>();
        }

        public int? ReportsToEmployeeId { get; set; }
        public EmployeeBase ReportsToEmployee { get; set; }
        public ICollection<EmployeeBase> EmployeesSupervised { get; set; }
    }

    public class EmployeeBase3 : EmployeeBase2
    {
        // Attention 17 - Employee, with self-referencing AND associated info from other entities
        // This resource model class includes data for external associations,
        // including "job duties" and "addresses"

        public EmployeeBase3()
        {
            JobDuties = new List<JobDutyBase>();
        }

        public AddressBase HomeAddress { get; set; }
        public AddressBase WorkAddress { get; set; }
        public ICollection<JobDutyBase> JobDuties { get; set; }
    }

    // In this use case, we will permit the names to be edited
    public class EmployeeEditNames
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }
    }

    // Attention 18 - Employee resource models that support the command pattern
    // Notice that we can use a custom shape for the data package to match our use case
    // Here, we need only the identifiers

    // In this use case, an employee's supervisor can be configured
    public class EmployeeSupervisor
    {
        public int Employee { get; set; }
        public int Supervisor { get; set; }
    }

    // In this use case, an employee's job duty can be configured
    public class EmployeeJobDuty
    {
        public int Employee { get; set; }
        public int JobDuty { get; set; }
    }

}
