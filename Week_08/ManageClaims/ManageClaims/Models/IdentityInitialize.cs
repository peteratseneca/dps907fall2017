using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// added...
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ManageClaims.Models
{
    public static class IdentityInitialize
    {
        // Attention 01 - Load user accounts when the app loads for the first time
        public static async void LoadUserAccounts()
        {
            // Get a reference to the objects we need
            var ds = new ApplicationDbContext();
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(ds));

            // Add the user(s) that the app needs when loaded for the first time
            // Change any of the data below to better match your app's needs

            // Attention 02 - This runs only if there are no existing user accounts
            if (userManager.Users.Count() == 0)
            {
                // User account manager...
                var uam = new ApplicationUser { UserName = "uam@example.com", Email = "uam@example.com" };
                var uamResult = await userManager.CreateAsync(uam, "Password123!");
                if (uamResult.Succeeded)
                {
                    // Add claims
                    await userManager.AddClaimAsync(uam.Id, new Claim(ClaimTypes.Email, "uam@example.com"));
                    await userManager.AddClaimAsync(uam.Id, new Claim(ClaimTypes.Role, "UserAccountManager"));
                    await userManager.AddClaimAsync(uam.Id, new Claim(ClaimTypes.GivenName, "User Account"));
                    await userManager.AddClaimAsync(uam.Id, new Claim(ClaimTypes.Surname, "Manager"));
                }

                // Developer/programmer...
                var dev = new ApplicationUser { UserName = "dev@example.com", Email = "dev@example.com" };
                var devResult = await userManager.CreateAsync(dev, "Password123!");
                if (devResult.Succeeded)
                {
                    // Add claims
                    await userManager.AddClaimAsync(dev.Id, new Claim(ClaimTypes.Email, "dev@example.com"));
                    await userManager.AddClaimAsync(dev.Id, new Claim(ClaimTypes.Role, "Developer"));
                    await userManager.AddClaimAsync(dev.Id, new Claim(ClaimTypes.GivenName, "App"));
                    await userManager.AddClaimAsync(dev.Id, new Claim(ClaimTypes.Surname, "Developer"));
                }
            }
        }

        // Load app claims
        // (write your code here)
        // (get a reference to the manager object, and then call its methods)


    }
}
