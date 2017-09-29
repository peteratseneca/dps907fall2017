using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// added...
using System.ComponentModel.DataAnnotations;

namespace AssociationsIntro.Controllers
{
    // Employee resource models, Add, Base, and WithCustomers

    public class EmployeeAdd
    {
        [Required, StringLength(20)]
        public string LastName { get; set; }

        [Required, StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string Title { get; set; }

        public int? ReportsTo { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? HireDate { get; set; }

        [Required, StringLength(70)]
        public string Address { get; set; }

        [Required, StringLength(40)]
        public string City { get; set; }

        [Required, StringLength(40)]
        public string State { get; set; }

        [Required, StringLength(40)]
        public string Country { get; set; }

        [Required, StringLength(10)]
        public string PostalCode { get; set; }

        [Required, StringLength(24)]
        public string Phone { get; set; }

        [Required, StringLength(24)]
        public string Fax { get; set; }

        [Required, StringLength(60)]
        public string Email { get; set; }
    }

    // Inheritance works in this simple situation
    public class EmployeeBase : EmployeeAdd
    {
        // For most Chinook model classes, must use the [Key] attribute in the resource model classes for the identifier property

        [Key]
        public int EmployeeId { get; set; }
    }

    // Inheritance works in this simple situation
    public class EmployeeWithCustomers : EmployeeBase
    {
        public EmployeeWithCustomers()
        {
            // When there is a collection, initialize it in the default constructor
            Customers = new List<CustomerBase>();
        }

        // Notice IEnumerable (not ICollection); do NOT use "virtual" keyword
        public IEnumerable<CustomerBase> Customers { get; set; }
    }

    public class EmployeeEditContactInfo
    {
        // Will allow only specific properties to be edited

        [Key]
        public int EmployeeId { get; set; }

        [Required, StringLength(24)]
        public string Phone { get; set; }

        [Required, StringLength(24)]
        public string Fax { get; set; }

        [Required, StringLength(60)]
        public string Email { get; set; }
    }
}
