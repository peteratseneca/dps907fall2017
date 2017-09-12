using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// added...
using System.ComponentModel.DataAnnotations;

namespace AllHttpMethods.Controllers
{
    // Attention 02 - Resource model classes
    // We need three of them:
    // 1. Add
    // 2. Base
    // 3. Edit

    // Remember - tip/hint - to ease the process of writing resource model classes,
    // copy the design model class, then make changes

    public class EmployeeAdd
    {
        public EmployeeAdd()
        {
            BirthDate = DateTime.Now.AddYears(-30);
            HireDate = DateTime.Now.AddYears(-1);
        }

        [Required]
        [StringLength(20)]
        public string LastName { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string Title { get; set; }

        // We will leave this property as-is, so entering a value will be optional
        public int? ReportsTo { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? HireDate { get; set; }

        // Added "Required" to the postal address related properties
        // The design model class doesn't have this annotation
        // However, adding it here improves the quality of the incoming data

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

        [StringLength(24)]
        public string Phone { get; set; }

        [StringLength(24)]
        public string Fax { get; set; }

        [StringLength(60)]
        public string Email { get; set; }
    }

    // Inherits from EmployeeAdd
    public class EmployeeBase : EmployeeAdd
    {
        [Key]
        public int EmployeeId { get; set; }
    }

    // Allow editing of the contact info ONLY
    public class EmployeeEditContactInfo
    {
        [Key]
        public int EmployeeId { get; set; }

        [StringLength(24)]
        public string Phone { get; set; }

        [StringLength(24)]
        public string Fax { get; set; }

        [StringLength(60)]
        public string Email { get; set; }
    }
}
