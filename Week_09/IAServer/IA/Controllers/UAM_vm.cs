using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// added...
using System.ComponentModel.DataAnnotations;

namespace IA.Controllers
{
    // UAM = User Account Management
    // Resource model classes that help with this task

    public class MSUserAccountBase
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class MSUserAccountWithClaims : MSUserAccountBase
    {
        public IEnumerable<MSClaimBase> Claims { get; set; }
    }

    public class MSClaimAdd
    {
        [Required, StringLength(200)]
        public string ClaimType { get; set; }

        [Required, StringLength(200)]
        public string ClaimValue { get; set; }

        // Info obtained from the database table definition
        [Required, StringLength(128)]
        public string UserId { get; set; }
    }

    public class MSClaimBase : MSClaimAdd
    {
        public int Id { get; set; }
    }
}
