using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// new...
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IA.Controllers
{
    [Authorize]
    public class MaintenanceController : Controller
    {
        // Default constructor
        // Invoke the next constructor, by passing in an initialized UserManager<TUser> object
        // This coding style is fairly common, as it allows an external caller
        // to initialize this class by using their choice of constructors
        // The end result is that the class gets initialized correctly
        public MaintenanceController() :
            this(new UserManager<Models.ApplicationUser>(new UserStore<Models.ApplicationUser>(new Models.ApplicationDbContext())))
        { }

        public MaintenanceController(UserManager<Models.ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        // Property to hold the user manager
        private UserManager<Models.ApplicationUser> UserManager { get; set; }

        // This method will display a list of current user accounts

        // GET: Maintenance
        public ActionResult Index()
        {
            // Container to hold the user names
            var accountList = new List<string>();

            // Get a reference to the application's user manager
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

            // Go through the users, and extract their names
            foreach (var user in userManager.Users)
            {
                accountList.Add(user.UserName);
            }

            // Package it for the view
            ViewBag.UserList = accountList.OrderBy(username => username);

            return View();
        }

        // This enables the admin to delete an account

        // GET: Maintenance/Delete/user@example.com/
        [Authorize(Roles = "UserAccountManager")]
        public ActionResult Delete(string userName)
        {
            return View(new UserDelete { UserName = userName });
        }

        // POST: Maintenance/Delete?username=user@example.com
        [HttpPost]
        [Authorize(Roles = "UserAccountManager")]
        public async Task<ActionResult> Delete(string userName, FormCollection collection)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                // Attempt to fetch the user account
                var applicationUser =
                    UserManager.Users.SingleOrDefault(au => au.UserName == userName);

                if (applicationUser == null) { ModelState.AddModelError("", "User not found"); }

                // Attempt to delete the user
                var result = await UserManager.DeleteAsync(applicationUser);

                if (result.Succeeded)
                {
                    // Good result, redirect...
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to delete user");
                    var errors = string.Join(", ", result.Errors);
                    ModelState.AddModelError("", errors);
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Must identify a user");
                return View();
            }
        }

    }

    public class UserDelete
    {
        [Display(Name = "User account")]
        [Required]
        public string UserName { get; set; }
    }

}
