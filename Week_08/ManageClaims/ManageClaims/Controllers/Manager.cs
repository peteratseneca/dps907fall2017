using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AutoMapper;
using ManageClaims.Models;

namespace ManageClaims.Controllers
{
    public class Manager
    {
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

                cfg.CreateMap<Models.AppClaim, Controllers.AppClaimBase>();
                cfg.CreateMap<Controllers.AppClaimAdd, Models.AppClaim>();
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
        // AppClaim

        // Attention 09 - Manager methods that perform CRUD tasks on custom claims

        // AppClaimGetAll
        public IEnumerable<AppClaimBase> AppClaimGetAll()
        {
            var c = ds.AppClaims.OrderBy(a => a.ClaimType).ThenBy(a => a.ClaimValue);

            return mapper.Map<IEnumerable<AppClaimBase>>(c);
        }

        // AppClaimGetAllActive
        public IEnumerable<AppClaimBase> AppClaimGetAllActive()
        {
            var c = ds.AppClaims
                .Where(a => a.DateRetired == null)
                .OrderBy(a => a.ClaimType).ThenBy(a => a.ClaimValue);

            return mapper.Map<IEnumerable<AppClaimBase>>(c);
        }

        // AppClaimGetById
        public AppClaimBase AppClaimGetById(int id)
        {
            // Attempt to fetch the object
            var o = ds.AppClaims.Find(id);

            return (o == null) ? null : mapper.Map<AppClaimBase>(o);
        }

        // AppClaimGetByMatch
        public AppClaimBase AppClaimGetByMatch(string claimType = "", string claimValue = "")
        {
            // Clean the incoming data
            claimType = claimType.Trim().ToLower();
            claimValue = claimValue.Trim().ToLower();

            // Attempt to fetch the object
            var o = ds.AppClaims
                .SingleOrDefault(a => a.ClaimType.ToLower() == claimType && a.ClaimValue.ToLower() == claimValue);

            return (o == null) ? null : mapper.Map<AppClaimBase>(o);
        }

        // AppClaimGetAllRoles
        public IEnumerable<AppClaimBase> AppClaimGetAllRoles()
        {
            var c = ds.AppClaims
                .Where(a => a.DateRetired == null)
                .Where(a => a.ClaimType.ToLower() == "role")
                .OrderBy(a => a.ClaimType).ThenBy(a => a.ClaimValue);

            return mapper.Map<IEnumerable<AppClaimBase>>(c);
        }

        // AppClaimGetByType case insensitive, active
        public IEnumerable<AppClaimBase> AppClaimGetByType(string claimType = "")
        {
            throw new NotImplementedException();
        }

        // AppClaimGetByValue case insensitive, active
        public IEnumerable<AppClaimBase> AppClaimGetByValue(string claimValue = "")
        {
            throw new NotImplementedException();
        }

        // AppClaimAdd
        public AppClaimBase AppClaimAdd(AppClaimAdd newItem)
        {
            // Maybe check for a retired match and resurrect it
            // Also check for existing match - keep them unique

            // Initial version of the method, without the fixes above...
            // Attempt to add the object
            var addedItem = ds.AppClaims.Add(mapper.Map<AppClaim>(newItem));

            // Help configure a role claim with the official URI
            if (addedItem.ClaimType.ToLower() == "role")
            {
                addedItem.ClaimTypeUri = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
            }

            ds.SaveChanges();

            // Return the result
            return (addedItem == null) ? null : mapper.Map<AppClaimBase>(addedItem);
        }

        // AppClaimEdit description, types and values, active
        public AppClaimBase AppClaimEdit(AppClaimEdit editedItem)
        {
            // Active only
            // Update the DateUpdated value
            throw new NotImplementedException();
        }

        // AppClaimDelete which doesn't really delete, could be a command
        public bool AppClaimDelete(int id)
        {
            // Active only
            throw new NotImplementedException();
        }

    }
}