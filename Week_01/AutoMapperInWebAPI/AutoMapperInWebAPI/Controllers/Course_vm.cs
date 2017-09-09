using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// added...
using System.ComponentModel.DataAnnotations;

namespace AutoMapperInWebAPI.Controllers
{
    public class CourseAdd
    {
        public CourseAdd()
        {
            DateCreated = DateTime.Now;
        }

        [Required, StringLength(6)]
        public string Code { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        public int Hours { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class CourseBase : CourseAdd
    {
        public int Id { get; set; }
    }

}
