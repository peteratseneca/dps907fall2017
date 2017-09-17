using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace AssociationsOther.Controllers
{
    // Attention 25 - JobDuty resource models

    public class JobDutyAdd
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(1000)]
        public string Description { get; set; }
    }

    // Attention 26 - JobDutyBase - notice that we can use this for the "edit" function too
    public class JobDutyBase : JobDutyAdd
    {
        public int Id { get; set; }
    }

    public class JobDutyFull : JobDutyBase
    {
        // Attention 27 - JobDuty, with associated employee info
        public JobDutyFull()
        {
            Employees = new List<EmployeeBase>();
        }

        public IEnumerable<EmployeeBase> Employees { get; set; }
    }

}