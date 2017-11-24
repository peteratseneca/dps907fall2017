using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
// added...
using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace ManageClaims.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false) { }

        // DbSet properties
        public DbSet<AppClaim> AppClaims { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    // ############################################################
    // AppClaim

    // Attention 06 - AppClaim class describes a custom claim for the app

    public class AppClaim
    {
        public AppClaim()
        {
            DateCreated = DateTime.Now;
            DateUpdated = DateCreated;
        }

        /// <summary>
        /// Claim identifier (for storage only)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date and time that the claim was created
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Date and time that the claim was updated
        /// </summary>
        public DateTime DateUpdated { get; set; }

        /// <summary>
        /// Date and time that the claim was retired and removed from use/service
        /// </summary>
        public DateTime? DateRetired { get; set; }

        /// <summary>
        /// Claim description
        /// </summary>
        [Required, StringLength(200)]
        public string Description { get; set; } = "(none)";

        /// <summary>
        /// Claim type
        /// </summary>
        [Required, StringLength(100)]
        public string ClaimType { get; set; } = "(none)";

        /// <summary>
        /// Claim type, as a URI
        /// </summary>
        [Required, StringLength(200)]
        public string ClaimTypeUri { get; set; } = "(none)";

        /// <summary>
        /// Claim value
        /// </summary>
        [Required, StringLength(100)]
        public string ClaimValue { get; set; } = "(none)";
    }

}