using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AutoMapper;
using AllHttpMethods.Models;
using System.Web.Http;
using System.Net.Http;

namespace AllHttpMethods.Controllers
{
    public class Manager
    {
        // Reference to the data context
        private DataContext ds = new DataContext();

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

                cfg.CreateMap<Controllers.EmployeeAdd, Models.Employee>();

                cfg.CreateMap<Models.Employee, Controllers.EmployeeBase>();
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

        // Add methods below
        // Controllers will call these methods
        // Ensure that the methods accept and deliver ONLY view model objects and collections
        // The collection return type is almost always IEnumerable<T>

        // Suggested naming convention: Entity + task/action
        // For example:
        // ProductGetAll()
        // ProductGetById()
        // ProductAdd()
        // ProductEdit()
        // ProductDelete()



        // ############################################################
        // Employees

        // Attention 03 - Manager methods, which perform data service tasks
        // Notice that the pattern is similar to what you did in web apps

        // All employees
        public IEnumerable<EmployeeBase> EmployeeGetAll()
        {
            // Fetch the collection
            var c = ds.Employees.OrderBy(e => e.LastName).ThenBy(e => e.FirstName);

            // Return the results as a collection based on a resource model class
            return mapper.Map<IEnumerable<EmployeeBase>>(c);
        }
        
        // Employee by identifier
        public EmployeeBase EmployeeGetById(int id)
        {
            // Attempt to fetch the object
            var o = ds.Employees.Find(id);

            // Return the result, or null if not found
            return (o == null) ? null : mapper.Map<EmployeeBase>(o);
        }

        // Add employee
        public EmployeeBase EmployeeAdd(EmployeeAdd newItem)
        {
            // Attempt to add the object
            var addedItem = ds.Employees.Add(mapper.Map<Employee>(newItem));
            ds.SaveChanges();

            // Return the result, or null if there was an error
            return (addedItem == null) ? null : mapper.Map<EmployeeBase>(addedItem);
        }

        // Edit employee - contact info only
        public EmployeeBase EmployeeEditContactInfo(EmployeeEditContactInfo editedItem)
        {
            // Ensure that we can continue
            if (editedItem == null) { return null; }

            // Attempt to fetch the object
            var storedItem = ds.Employees.Find(editedItem.EmployeeId);

            if (storedItem == null)
            {
                return null;
            }
            else
            {
                // Fetch the object from the data store - ds.Entry(storedItem)
                // Get its current values collection - .CurrentValues
                // Set those to the edited values - .SetValues(editedItem)
                ds.Entry(storedItem).CurrentValues.SetValues(editedItem);
                // The SetValues() method ignores missing properties and navigation properties
                ds.SaveChanges();

                return mapper.Map<EmployeeBase>(storedItem);
            }
        }

        // Delete employee
        public void EmployeeDelete(int id)
        {
            // Attempt to fetch the existing item
            var storedItem = ds.Employees.Find(id);

            // Interim coding strategy...

            if (storedItem == null)
            {
                // Throw an exception, and you will learn how soon
            }
            else
            {
                try
                {
                    // If this fails, throw an exception (as above)
                    // This implementation just prevents an error from bubbling up
                    ds.Employees.Remove(storedItem);
                    ds.SaveChanges();
                }
                catch (Exception) { }
            }
        }



        // ############################################################
        // Example

        // Method templates, used by the ExampleController class

        public IEnumerable<string> ExampleGetAll()
        {
            return new List<string> { "hello", "world" };
        }

        public string ExampleGetById(int id)
        {
            return $"id {id} was requested";
        }

        public string ExampleAdd(string newItem)
        {
            return $"new item {newItem} was added";
        }

        public string ExampleEditSomething(string editedItem)
        {
            return $"item was edited with {editedItem}";
        }

        public bool ExampleDelete(int id)
        {
            return true;
        }

    }
}