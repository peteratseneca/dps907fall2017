using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// added...
using AutoMapperInWebAPI.Models;
using AutoMapper;

namespace AutoMapperInWebAPI.Controllers
{
    public class Manager
    {
        // Attention 02 - Manager class features...

        // Constructor
        // Data context class reference
        // AutoMapper instance definition
        // Other data-handling configuration

        // Reference to the data context
        private ApplicationDbContext ds = new ApplicationDbContext();

        // AutoMapper components
        MapperConfiguration config;
        public IMapper mapper;

        public Manager()
        {
            // If necessary, add your own code here

            // Configure AutoMapper...
            config = new MapperConfiguration(cfg =>
            {
                // Define the mappings below, for example...
                // cfg.CreateMap<SourceType, DestinationType>();
                // cfg.CreateMap<Employee, EmployeeBase>();

                cfg.CreateMap<Controllers.CourseAdd, Models.Course>();
                cfg.CreateMap<Models.Course, Controllers.CourseBase>();
            });

            mapper = config.CreateMapper();


            // Data-handling configuration

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }

        // Attention 03 - Data service operations / tasks

        // All look just like they did in ASP.NET MVC web apps

        // For those with OLD AutoMapper knowledge, remember that all mapping tasks
        // are done with the lower-case instance "mapper" object, defined above in the constructor

        // Get all
        // Get one
        // Add new

        public IEnumerable<CourseBase> CourseGetAll()
        {
            return mapper.Map<IEnumerable<CourseBase>>(ds.Courses.OrderBy(c => c.Title));
        }

        public CourseBase CourseGetOne(int id)
        {
            throw new NotImplementedException();
        }

        public CourseBase CourseAdd(CourseAdd newItem)
        {
            throw new NotImplementedException();
        }

        // Attention 04 - Programmatically-generated objects

        // Can do this in one method, or in several
        // Call the method(s) from a controller method

        public bool LoadData()
        {
            // Return immediately if there's existing data
            if (ds.Courses.Count() > 0) { return false; }

            // Otherwise, add objects...

            ds.Courses.Add(new Course { Code = "BTP100", Title = "Programming Fundamentals Using C", Hours = 6 });
            ds.Courses.Add(new Course { Code = "BTP105", Title = "Computer Principles for Programmers", Hours = 6 });
            ds.Courses.Add(new Course { Code = "BTO120", Title = "Operating Systems for Programmers - Unix", Hours = 6 });

            return ds.SaveChanges() > 0 ? true : false;
        }
    }
}
