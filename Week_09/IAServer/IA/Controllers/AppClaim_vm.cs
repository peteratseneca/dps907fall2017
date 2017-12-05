using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// added...
using System.ComponentModel.DataAnnotations;

namespace IA.Controllers
{
    public class AppClaimAdd
    {
        /// <summary>
        /// Claim description
        /// </summary>
        [Required, StringLength(200)]
        [Display(Name = "Description and purpose")]
        public string Description { get; set; } = "(none)";

        /// <summary>
        /// Claim type
        /// </summary>
        [Required, StringLength(100)]
        [Display(Name = "Claim type")]
        public string ClaimType { get; set; } = "(none)";

        /// <summary>
        /// Claim type, as a URI
        /// </summary>
        [Required, StringLength(200)]
        [Display(Name = "Claim type URI")]
        public string ClaimTypeUri { get; set; } = "(none)";

        /// <summary>
        /// Claim value
        /// </summary>
        [Required, StringLength(100)]
        [Display(Name = "Claim value")]
        public string ClaimValue { get; set; } = "(none)";
    }

    public class AppClaimBase : AppClaimAdd
    {
        /// <summary>
        /// Claim identifier (for storage only)
        /// </summary>
        [Display(Name = "Claim identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Date and time that the claim was created
        /// </summary>
        [Display(Name = "Date created")]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Date and time that the claim was updated
        /// </summary>
        [Display(Name = "Date updated")]
        public DateTime DateUpdated { get; set; }

        /// <summary>
        /// Date and time that the claim was retired and removed from use/service
        /// </summary>
        [Display(Name = "Date retired/removed")]
        public DateTime? DateRetired { get; set; }

        /// <summary>
        /// Is active? Or, has this claim been retired (removed from service)?
        /// </summary>
        [Display(Name = "Is this in active use?")]
        public bool IsActive
        {
            get
            {
                return (DateTime.Now > DateRetired.GetValueOrDefault()) ? true : false;
            }
        }

        /// <summary>
        /// Is a role claim?
        /// </summary>
        [Display(Name = "Is this a role claim?")]
        public bool IsRoleClaim
        {
            get
            {
                return (ClaimType.ToLower() == "role") ? true : false;
            }
        }
    }

    public class AppClaimEdit
    {
        // descr, types, value
    }

}
