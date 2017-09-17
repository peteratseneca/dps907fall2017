using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// more...
using System.ComponentModel.DataAnnotations;

namespace AssociationsOther.Models
{
    // Attention 01 - Employee entity, has many kinds of associations
    // Self-referencing to-one (i.e. the employee's supervisor)
    // Self-referencing to-many (i.e. the employees that report to this employee)
    // To-one, with Address
    // To-many, with JobDuty

    public class Employee
    {
        public Employee()
        {
            this.EmployeesSupervised = new List<Employee>();
            this.JobDuties = new List<JobDuty>();
        }

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        // Attention 02 - Details for one-to-one, to the same entity (self-referencing)
        // Include an int property to hold the identifier of the pointed-to object
        // It must be nullable, because it is optional (in most situations)
        public int? ReportsToEmployeeId { get; set; }
        // Next, include a nav property to this class 
        public Employee ReportsToEmployee { get; set; }

        // Attention 03 - Details for one-to-many, to the same entity (self-referencing)
        // An employee who is a supervisor has a collection of employees
        // who report to the supervisor
        public ICollection<Employee> EmployeesSupervised { get; set; }

        // Attention 04 - Details for one-to-one, to another entity
        // An employee can OPTIONALLY have address properties
        // This is the "principal" end of the Employee-Address association
        public Address HomeAddress { get; set; }
        public Address WorkAddress { get; set; }

        // Attention 05 - Details for many-to-many, to another entity
        public ICollection<JobDuty> JobDuties { get; set; }
    }

    // Attention 06 - Address entity, has to-one association with employee

    // Rules...
    // An employee can OPTIONALLY have address properties (principal)
    // An address MUST be associated with an employee (dependent)

    public class Address
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        [Required, StringLength(100)]
        public string CityAndProvince { get; set; }

        [Required, StringLength(20)]
        public string PostalCode { get; set; }

        public int? EmployeeId { get; set; }

        // Attention 07 - Details for one-to-one, to another entity
        // An address MUST be associated with an employee (dependent)
        // This is the "dependent" end of the Employee-Address association
        // Therefore, it MUST use the [Required] data annotation
        [Required]
        public Employee Employee { get; set; }
    }

    // Attention 08 - JobDuty entity, has to-many association with employee
    public class JobDuty
    {
        public JobDuty()
        {
            this.Employees = new List<Employee>();
        }

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(1000)]
        public string Description { get; set; }

        // Attention 09 - Details for many-to-many, to another entity
        public ICollection<Employee> Employees { get; set; }
    }
}
