using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace AssociationsOther.Controllers
{
    // Attention 20 - Address resource models

    public class AddressAdd
    {
        // This value should be "Home" or "Work"
        [Required, StringLength(100)]
        public string AddressType { get; set; }

        [Required, StringLength(100)]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        [Required, StringLength(100)]
        public string CityAndProvince { get; set; }

        [Required, StringLength(20)]
        public string PostalCode { get; set; }

        [Range(1, UInt32.MaxValue)]
        public int EmployeeId { get; set; }
    }

    public class AddressBase
    {
        // Attention 21 - AddressBase does not use inheritance, by design
        // Why? We do not want the "AddressType" property in this resource model

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        [Required, StringLength(100)]
        public string CityAndProvince { get; set; }

        [Required, StringLength(20)]
        public string PostalCode { get; set; }

        // Remember... we cannot use "Required" on a value type
        // However, we can use "Range", and have the same effect
        [Range(1, UInt32.MaxValue)]
        public int EmployeeId { get; set; }
    }

    public class AddressFull : AddressBase
    {
        // Attention 22 - Address, includes associated employee info
        public EmployeeBase Employee { get; set; }
    }

    public class AddressEdit
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        [Required, StringLength(100)]
        public string CityAndProvince { get; set; }

        [Required, StringLength(20)]
        public string PostalCode { get; set; }
    }

}