using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// added...
using System.ComponentModel.DataAnnotations;

namespace AssociationsIntro.Controllers
{
    // Customer resource models, Add, Base, WithEmployee, and EditContactInfo

    public class CustomerAdd
    {
        [Required, StringLength(40)]
        public string FirstName { get; set; }

        [Required, StringLength(20)]
        public string LastName { get; set; }

        [StringLength(80)]
        public string Company { get; set; }

        [StringLength(70)]
        public string Address { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        [StringLength(40)]
        public string State { get; set; }

        [StringLength(40)]
        public string Country { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(24)]
        public string Phone { get; set; }

        [StringLength(24)]
        public string Fax { get; set; }

        [Required, StringLength(60)]
        public string Email { get; set; }

        // This is the identifier for the employee who looks after the customer
        // I do not know why the database designer used non-matching names
        // SupportRepId is supposed to match to the Employee identifier
        // Anyway, you will have to be ready for things like this in existing code
        public int? SupportRepId { get; set; }

    }

    public class CustomerBase : CustomerAdd
    {
        // Notice, again, the [Key] attribute

        [Key]
        public int CustomerId { get; set; }
    }

    public class CustomerWithEmployee : CustomerBase
    {
        public EmployeeBase Employee { get; set; }
    }

    public class CustomerEditContactInfo
    {
        // Will allow only specific properties to be edited

        [Key]
        public int CustomerId { get; set; }

        [StringLength(24)]
        public string Phone { get; set; }

        [StringLength(24)]
        public string Fax { get; set; }

        [Required, StringLength(60)]
        public string Email { get; set; }
    }

    public class CustomerEditAddress
    {
        // Not used in this app, but it shows that you can create
        // multiple resource models to match the needs of the use cases

        [Key]
        public int CustomerId { get; set; }

        [StringLength(70)]
        public string Address { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        [StringLength(40)]
        public string State { get; set; }

        [StringLength(40)]
        public string Country { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }
    }
}
