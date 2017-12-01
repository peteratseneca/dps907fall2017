using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AutoMapper;
using SecuredCustomer.Models;

namespace SecuredCustomer.Controllers
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

                cfg.CreateMap<Models.Employee, Controllers.EmployeeBase>();
                cfg.CreateMap<Models.Employee, Controllers.EmployeeWithCustomers>();
                cfg.CreateMap<Controllers.EmployeeAdd, Models.Employee>();

                cfg.CreateMap<Models.Customer, Controllers.CustomerBase>();
                cfg.CreateMap<Models.Customer, Controllers.CustomerWithEmployee>();

                cfg.CreateMap<Models.Customer, Controllers.CustomerEditContactInfo>();

                cfg.CreateMap<Controllers.CustomerAdd, Models.Customer>();
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
        // Customer

        // Customer get all and get one methods - very familiar

        public IEnumerable<CustomerBase> CustomerGetAll()
        {
            var c = ds.Customers.OrderBy(cu => cu.Company).ThenBy(cu => cu.LastName).ThenBy(cu => cu.FirstName);

            return mapper.Map<IEnumerable<CustomerBase>>(c);
        }

        public CustomerBase CustomerGetById(int id)
        {
            // Attempt to fetch the object
            var o = ds.Customers.Find(id);

            return (o == null) ? null : mapper.Map<CustomerBase>(o);
        }

        // Customer get one with associated object, new

        public CustomerWithEmployee CustomerGetByIdWithEmployee(int id)
        {
            // Attempt to fetch the object
            var o = ds.Customers.Include("Employee")
                .SingleOrDefault(c => c.CustomerId == id);

            return (o == null) ? null : mapper.Map<CustomerWithEmployee>(o);
        }

        // Customer add one

        public CustomerBase CustomerAdd(CustomerAdd newItem)
        {
            // To add a customer, we MUST have an Employee identifier
            // We must validate that employee first by attempting to fetch it
            // If successful, then we can continue

            // Attempt to find the associated object
            var a = ds.Employees.Find(newItem.SupportRepId);

            if (a == null)
            {
                return null;
            }
            else
            {
                // Attempt to add the object
                var addedItem = ds.Customers.Add(mapper.Map<Customer>(newItem));
                // Set the associated item property
                addedItem.Employee = a;
                ds.SaveChanges();

                // Return the result, or null if there was an error
                return (addedItem == null) ? null : mapper.Map<CustomerBase>(addedItem);
            }
        }

        public void CustomerDelete(int id)
        {
            // Attempt to fetch the existing item
            var storedItem = ds.Customers.Find(id);

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
                    ds.Customers.Remove(storedItem);
                    ds.SaveChanges();
                }
                catch (Exception) { }
            }
        }

    }
}