using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AssociationsOther.Models;
using AutoMapper;

namespace AssociationsOther.Controllers
{
    public class Manager
    {
        // Facade reference
        ApplicationDbContext ds = new ApplicationDbContext();

        // AutoMapper components
        MapperConfiguration config;
        public IMapper mapper;

        public Manager()
        {
            // If necessary, add your own code here

            // Configure Automapper...
            config = new MapperConfiguration(cfg =>
            {
                // Define the mappings below, for example...
                // cfg.CreateMap<SourceType, DestinationType>();
                // cfg.CreateMap<Employee, EmployeeBase>();

                cfg.CreateMap<Models.Employee, Controllers.EmployeeBase>();
                cfg.CreateMap<Models.Employee, Controllers.EmployeeBase2>();
                cfg.CreateMap<Models.Employee, Controllers.EmployeeBase3>();
                cfg.CreateMap<Controllers.EmployeeAdd, Models.Employee>();

                cfg.CreateMap<Models.Address, Controllers.AddressBase>();
                cfg.CreateMap<Models.Address, Controllers.AddressFull>();
                cfg.CreateMap<Controllers.AddressAdd, Models.Address>();

                cfg.CreateMap<Models.JobDuty, Controllers.JobDutyBase>();
                cfg.CreateMap<Models.JobDuty, Controllers.JobDutyFull>();
                cfg.CreateMap<Controllers.JobDutyAdd, Models.JobDuty>();
            });

            mapper = config.CreateMapper();

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

        public IEnumerable<EmployeeBase> EmployeeGetAll()
        {
            var c = ds.Employees
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName);

            return mapper.Map<IEnumerable<EmployeeBase>>(c);
        }

        // No associated data
        public EmployeeBase EmployeeGetById(int id)
        {
            var o = ds.Employees.Find(id);

            return (o == null) ? null : mapper.Map<EmployeeBase>(o);
        }

        // Attention 30 - Employee get by identifier, with self-associated info
        // With self associated data (other Employee objects)
        public EmployeeBase2 EmployeeGetByIdWithEmployeeInfo(int id)
        {
            var o = ds.Employees
                .Include("ReportsToEmployee")
                .Include("EmployeesSupervised")
                .SingleOrDefault(e => e.Id == id);

            return (o == null) ? null : mapper.Map<EmployeeBase2>(o);
        }

        // Attention 31 - Employee get by identifier, with all associated info
        // With all associated data
        public EmployeeBase3 EmployeeGetByIdWithAllInfo(int id)
        {
            var o = ds.Employees
                .Include("ReportsToEmployee")
                .Include("EmployeesSupervised")
                .Include("HomeAddress")
                .Include("WorkAddress")
                .Include("JobDuties")
                .SingleOrDefault(e => e.Id == id);

            return (o == null) ? null : mapper.Map<EmployeeBase3>(o);
        }

        public EmployeeBase EmployeeAdd(EmployeeAdd newItem)
        {
            // Ensure that we can continue
            if (newItem == null)
            {
                return null;
            }
            else
            {
                // Add the new object
                Employee addedItem = mapper.Map<Employee>(newItem);

                ds.Employees.Add(addedItem);
                ds.SaveChanges();

                // Return the object
                return mapper.Map<EmployeeBase>(addedItem);
            }
        }

        public EmployeeBase EmployeeEditNames(EmployeeEditNames editedItem)
        {
            // Ensure that we can continue
            if (editedItem == null)
            {
                return null;
            }

            // Attempt to fetch the underlying object
            var storedItem = ds.Employees.Find(editedItem.Id);

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
        // These are COMMANDS
        // They affect an employee object
        // Notice that they are idempotent 

        // Attention 32 - Methods that implement the command pattern for emplooyee tasks
        // Set and cler the employee's supervisor
        // Set and clear a job duty

        public void SetEmployeeSupervisor(EmployeeSupervisor item)
        {
            // Attention 33 - Must get a valid reference to both objects before continuing

            // Get a reference to the employee
            var employee = ds.Employees.Find(item.Employee);
            if (employee == null) { return; }

            // Get a reference to the supervisor
            var supervisor = ds.Employees.Find(item.Supervisor);
            if (supervisor == null) { return; }

            // Make the changes, save, and exit
            employee.ReportsToEmployee = supervisor;
            employee.ReportsToEmployeeId = supervisor.Id;
            ds.SaveChanges();
        }

        public void ClearEmployeeSupervisor(EmployeeSupervisor item)
        {
            // Get a reference to the employee
            var employee = ds.Employees.Find(item.Employee);
            if (employee == null) { return; }

            // Get a reference to the supervisor
            var supervisor = ds.Employees.Find(item.Supervisor);
            if (supervisor == null) { return; }

            // Make the changes, save, and exit
            if (employee.ReportsToEmployeeId == supervisor.Id)
            {
                employee.ReportsToEmployee = null;
                employee.ReportsToEmployeeId = null;
                ds.SaveChanges();
            }
        }

        public void SetEmployeeJobDuty(EmployeeJobDuty item)
        {
            // Get a reference to the employee
            // Must bring back its collection of job duties
            var employee = ds.Employees
                .Include("JobDuties")
                .SingleOrDefault(e => e.Id == item.Employee);
            if (employee == null) { return; }

            // Get a reference to the job duty
            var jobDuty = ds.JobDuties.Find(item.JobDuty);
            if (jobDuty == null) { return; }

            // Make the changes, save, and exit
            employee.JobDuties.Add(jobDuty);
            ds.SaveChanges();
        }

        public void ClearEmployeeJobDuty(EmployeeJobDuty item)
        {
            // Get a reference to the employee
            // Must bring back its collection of job duties
            var employee = ds.Employees
                .Include("JobDuties")
                .SingleOrDefault(e => e.Id == item.Employee);
            if (employee == null) { return; }

            // Get a reference to the job duty
            // Notice that we're getting it from the employee object (above)
            var jobDuty = employee.JobDuties
                .SingleOrDefault(j => j.Id == item.JobDuty);
            if (jobDuty == null) { return; }

            // Make the changes, save, and exit
            if (employee.JobDuties.Remove(jobDuty))
            {
                ds.SaveChanges();
            }
        }

        // ############################################################
        // Addresses

        public IEnumerable<AddressBase> AddressGetAll()
        {
            var c = ds.Addresses.OrderBy(a => a.PostalCode);

            return mapper.Map<IEnumerable<AddressBase>>(c);
        }

        // Include associated data
        public AddressFull AddressGetById(int id)
        {
            var o = ds.Addresses
                .Include("Employee")
                .SingleOrDefault(a => a.Id == id);

            return (o == null) ? null : mapper.Map<AddressFull>(o);
        }

        // Attention 35 - Address add, requires extra checking and processing
        public AddressBase AddressAdd(AddressAdd newItem)
        {
            // Ensure that we can continue
            if (newItem == null)
            {
                return null;
            }
            else
            {
                // Must validate "Home" or "Work" address type
                var isAddressTypeValid =
                    (newItem.AddressType.Trim().ToLower() == "home" || newItem.AddressType.Trim().ToLower() == "work")
                    ? true
                    : false;
                if (!isAddressTypeValid) { return null; }

                // Must validate the associated object
                var associatedItem = ds.Employees.Find(newItem.EmployeeId);
                if (associatedItem == null)
                {
                    return null;
                }

                // Add the new object

                // Build the Address object
                Address addedItem = mapper.Map<Address>(newItem);

                // Set its associated item identifier
                addedItem.EmployeeId = associatedItem.Id;

                // Now, look at this next task from the perspective of the employee object
                // Set the appropriate address object
                if (newItem.AddressType.Trim().ToLower() == "home")
                {
                    associatedItem.HomeAddress = addedItem;
                }
                else
                {
                    associatedItem.WorkAddress = addedItem;
                }

                ds.Addresses.Add(addedItem);
                ds.SaveChanges();

                // Return the object
                return mapper.Map<AddressBase>(addedItem);
            }
        }

        // Attention 36 - Address delete, also requires extra checking and processing
        public void AddressDelete(int id)
        {
            // Attempt to fetch the existing item
            var storedItem = ds.Addresses.Find(id);

            // Interim coding strategy...

            if (storedItem == null)
            {
                // Throw an exception, and you will learn how soon
            }
            else
            {
                try
                {
                    // Remove the reference to the address to be deleted
                    // Must include the associated objects, so that the
                    // changes take place on both ends of the association
                    var associatedItem = ds.Employees
                        .Include("HomeAddress")
                        .Include("WorkAddress")
                        .SingleOrDefault(e => e.Id == storedItem.EmployeeId);

                    // If this fails, throw an exception (as above)
                    // This implementation just prevents an error from bubbling up

                    storedItem.Employee = null;
                    storedItem.EmployeeId = null;
                    ds.Addresses.Remove(storedItem);
                    ds.SaveChanges();
                }
                catch (Exception) { }
            }
        }

        public AddressBase AddressEdit(AddressEdit editedItem)
        {
            // Ensure that we can continue
            if (editedItem == null)
            {
                return null;
            }

            // Attempt to fetch the underlying object
            // Must include the required associated object,
            // so that the SetValues() and SaveChanges() methods will work
            var storedItem = ds.Addresses
                .Include("Employee")
                .SingleOrDefault(e => e.Id == editedItem.Id);

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

                return mapper.Map<AddressBase>(storedItem);
            }
        }

        // ############################################################
        // JobDuty

        public IEnumerable<JobDutyBase> JobDutyGetAll()
        {
            var c = ds.JobDuties.OrderBy(j => j.Name);

            return mapper.Map<IEnumerable<JobDutyBase>>(c);
        }

        // Include associated data
        public IEnumerable<JobDutyFull> JobDutyGetAllWithEmployees()
        {
            var c = ds.JobDuties
                .Include("Employees")
                .OrderBy(j => j.Name);

            return mapper.Map<IEnumerable<JobDutyFull>>(c);
        }

        // Include associated data
        public JobDutyFull JobDutyGetById(int id)
        {
            var o = ds.JobDuties
                .Include("Employees")
                .SingleOrDefault(j => j.Id == id);

            return (o == null) ? null : mapper.Map<JobDutyFull>(o);
        }

        public JobDutyBase JobDutyAdd(JobDutyAdd newItem)
        {
            // Ensure that we can continue
            if (newItem == null)
            {
                return null;
            }
            else
            {
                // Add the new object
                JobDuty addedItem = mapper.Map<JobDuty>(newItem);

                ds.JobDuties.Add(addedItem);
                ds.SaveChanges();

                // Return the object
                return mapper.Map<JobDutyBase>(addedItem);
            }
        }

        public void JobDutyDelete(int id)
        {
            // Attempt to fetch the existing item
            // Must include the associated employees
            var storedItem = ds.JobDuties
                .Include("Employees")
                .SingleOrDefault(j => j.Id == id);

            // Interim coding strategy...

            if (storedItem == null)
            {
                // Throw an exception, and you will learn how soon
            }
            else
            {
                try
                {
                    // Allow delete only if it is not used by any employees
                    if (storedItem.Employees.Count == 0)
                    {
                        ds.JobDuties.Remove(storedItem);
                        ds.SaveChanges();
                    }
                }
                catch (Exception) { }
            }
        }

        public JobDutyBase JobDutyEdit(JobDutyBase editedItem)
        {
            // Ensure that we can continue
            if (editedItem == null)
            {
                return null;
            }

            // Attempt to fetch the underlying object
            var storedItem = ds.JobDuties.Find(editedItem.Id);

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

                return mapper.Map<JobDutyBase>(storedItem);
            }
        }

    }

}
