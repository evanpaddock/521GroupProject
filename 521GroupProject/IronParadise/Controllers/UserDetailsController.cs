using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IronParadise.Data;
using IronParadise.Models;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace IronParadise.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserDetailsController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<User> _userManager;

        public UserDetailsController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: UserDetails
        public async Task<IActionResult> Index()
        {
            var allUsers = _userManager.Users.ToList();

            // Create a model to hold user and role information
            var usersWithRoles = new List<UserDetails>();

            foreach (var user in allUsers)
            {
                // Retrieve roles for each user
                var role = await _userManager.GetRolesAsync(user);

                // Create a view model to hold user and role information
                var userWithRole = new UserDetails
                {
                    User = user,
                    RoleName = role[0],
                };

                usersWithRoles.Add(userWithRole);
            }

            // Do something with the users and roles (e.g., pass them to a view)
            return View(usersWithRoles);
        }

        // GET: UserDetails/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            var role = await _userManager.GetRolesAsync(user);

            // Create a view model to hold user and role information
            var userWithRole = new UserDetails
            {
                User = user,
                RoleName = role[0],
            };

            if (user != null)
            {
                // Do something with the user (e.g., pass it to a view)
                return View(userWithRole);
            }
            else
            {
                // Handle the case where the user with the specified ID was not found
                return NotFound();
            }
        }

        // GET: UserDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserDetails userDetails)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(userDetails.User.Email);
                if (user == null)
                {
                    user = userDetails.User;
                    user.UserName = user.Email;
                    user.NormalizedUserName = user.UserName.ToUpper();
                    user.NormalizedEmail = user.Email.ToUpper();
                    user.EmailConfirmed = true;

                    string password = "DefaultPassword123!";

                    await _userManager.CreateAsync(user, password);
                }

                await _userManager.AddToRoleAsync(user, "User");

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View("Error");
            }

        }

        // GET: UserDetails/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            var role = await _userManager.GetRolesAsync(user);

            if (user == null)
            {
                return NotFound();
            }

            // Create a view model to hold user and role information
            var userWithRole = new UserDetails
            {
                User = user,
                RoleName = role[0],
            };

            var allRoles = _roleManager.Roles.ToList();

            // Create a SelectList for roles
            var roleSelectList = new SelectList(allRoles, nameof(IdentityRole.Name), nameof(IdentityRole.Name));

            // Set ViewBag.Roles to the SelectList
            ViewBag.Roles = roleSelectList;

            return View(userWithRole);
        }

        // POST: UserDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserDetails userDetails, string roleName)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Find the user by Email or some other property you can use
                    var user = await _userManager.FindByIdAsync(userDetails.User.Id);

                    if (user == null)
                    {
                        return NotFound();
                    }

                    // Update user properties
                    user.FirstName = userDetails.User.FirstName;
                    user.LastName = userDetails.User.LastName;
                    user.Email = userDetails.User.Email;
                    user.NormalizedEmail = userDetails.User.Email.ToUpper();

                    // Get current roles of the user
                    var currentRoles = await _userManager.GetRolesAsync(user);

                    // Remove the user from all existing roles
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);

                    // Add the user to the selected role
                    await _userManager.AddToRoleAsync(user, userDetails.RoleName);

                    // Update the user
                    await _userManager.UpdateAsync(user);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency exception if needed
                    return NotFound();
                }
            }

            var allRoles = _roleManager.Roles.ToList();

            // Create a SelectList for roles
            var roleSelectList = new SelectList(allRoles, nameof(IdentityRole.Name), nameof(IdentityRole.Name));

            // Set ViewBag.Roles to the SelectList
            ViewBag.Roles = roleSelectList;
            return View(userDetails);
        }

        // GET: UserDetails/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            var role = await _userManager.GetRolesAsync(user);

            // Create a view model to hold user and role information
            var userWithRole = new UserDetails
            {
                User = user,
                RoleName = role[0],
            };

            if (user != null)
            {
                // Do something with the user (e.g., pass it to a view)
                return View(userWithRole);
            }
            else
            {
                // Handle the case where the user with the specified ID was not found
                return NotFound();
            }
        }

        // POST: UserDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Find the user by Email or some other property you can use
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // Get current roles of the user
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove the user from all existing roles
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Delete the user
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                // User deletion successful
                // You can redirect to a specific page or return a success message
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // User deletion failed
                // Handle errors, perhaps by displaying an error message
                // result.Errors will contain details about the errors
                return View("Error");
            }
        }
    }
}
