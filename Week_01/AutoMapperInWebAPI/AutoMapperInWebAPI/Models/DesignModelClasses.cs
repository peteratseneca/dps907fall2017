using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// added...
using System.ComponentModel.DataAnnotations;

namespace AutoMapperInWebAPI.Models
{
    public class Course
    {
        // Attention 01 - Data model for the app

        // This "Course" class defines an academic course here at the School of ICT

        // Must use data annotations

        // Must use a constructor to initialize dates, collections,
        // and optionally, other values you want initialized

        // Then, add a DbSet<T> property to the data context class (in IdentityModels.cs)

        public Course()
        {
            DateCreated = DateTime.Now;
        }

        public int Id { get; set; }

        [Required, StringLength(6)]
        public string Code { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        public int Hours { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
