using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AutoMapper;
using IA.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IA.Controllers
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

                // ############################################################
                // AppClaim

                cfg.CreateMap<Models.AppClaim, Controllers.AppClaimBase>();
                cfg.CreateMap<Controllers.AppClaimAdd, Models.AppClaim>();

                // ############################################################
                // User account management

                cfg.CreateMap<IdentityUser, Controllers.MSUserAccountBase>();
                cfg.CreateMap<IdentityUser, Controllers.MSUserAccountWithClaims>();
                cfg.CreateMap<IdentityUserClaim, Controllers.MSClaimBase>();
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

            // Special situations for the well-known claims if a short form is submitted
            claimType = (claimType == "role") ? ClaimTypes.Role : claimType;
            claimType = (claimType == "name") ? ClaimTypes.Name : claimType;
            claimType = (claimType == "emailaddress") ? ClaimTypes.Email : claimType;
            claimType = (claimType == "givenname") ? ClaimTypes.GivenName : claimType;
            claimType = (claimType == "surname") ? ClaimTypes.Surname : claimType;

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

        // ############################################################
        // User account management

        // All user accounts
        public IEnumerable<MSUserAccountBase> UAGetAll()
        {
            // Get a reference to the application's user manager
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

            // Return the entire user account collection, mapped
            return mapper.Map<IEnumerable<MSUserAccountBase>>(userManager.Users);
        }

        // One user account, by its 32-hex-character identifier
        public MSUserAccountWithClaims UAGetOneById(string id = "")
        {
            // Get a reference to the application's user manager
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

            // Attempt to locate the object
            var o = userManager.FindByIdAsync(id).Result;

            return (o == null) ? null : mapper.Map<MSUserAccountWithClaims>(o);
        }

        // One user account, by its email address
        public MSUserAccountWithClaims UAGetOneByEmail(string email = "")
        {
            // Get a reference to the application's user manager
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

            // Attempt to locate the object
            var o = userManager.FindByEmailAsync(email).Result;

            return (o == null) ? null : mapper.Map<MSUserAccountWithClaims>(o);
        }

        // Collection of zero or more user accounts that match surname (case-insensitive)
        public IEnumerable<MSUserAccountBase> UAGetAllBySurname(string surname = "")
        {
            // Get a reference to the application's user manager
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

            // Fetch the user accounts into an in-memory object graph so we can query it
            // without going back to the data store again and again
            var allUsers = userManager.Users.ToList();

            // Now, look for users where the surname claim matches the incoming value
            var matchingUsers = allUsers.Where
                (u => u.Claims.Any(c => c.ClaimType == ClaimTypes.Surname && c.ClaimValue.ToLower() == surname.Trim().ToLower()));

            return mapper.Map<IEnumerable<MSUserAccountBase>>(matchingUsers);
        }

    }
}