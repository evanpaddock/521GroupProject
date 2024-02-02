using IronParadise.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IronParadise.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string SeedUserAdminPW, string SeedUserManagerPW, string SeedUserPW)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if(context.Users.Count() != 0)
                {
                    return;
                }

                //create the user
                var adminID = await EnsureUser(serviceProvider, SeedUserAdminPW, "admin@ironparadise.com");
                //add user to role (and make sure role exists)
                await EnsureRole(serviceProvider, adminID, Constants.AdministratorsRole);
                await SeedDB(context, adminID, "dev", "admin", "admin@ironparadise.com");

                var managerID = await EnsureUser(serviceProvider, SeedUserManagerPW, "manager@ironparadise.com");
                await EnsureRole(serviceProvider, managerID, Constants.ManagersRole);
                await SeedDB(context, managerID, "dev", "manager", "manager@ironparadise.com");


                var user1ID = await EnsureUser(serviceProvider, SeedUserPW, "edpaddock@ironparadise.dev.com");
                await EnsureRole(serviceProvider, user1ID, Constants.User);
                await SeedDB(context, user1ID, "Evan", "Paddock", "edpaddock@ironparadise.dev.com");

                var user2ID = await EnsureUser(serviceProvider, SeedUserPW, "klknudson@ironparadise.dev.com");
                await EnsureRole(serviceProvider, user2ID, Constants.User);
                await SeedDB(context, user2ID, "Kyle", "Knudson", "klknudson@ironparadise.dev.com");


                var user3ID = await EnsureUser(serviceProvider, SeedUserPW, "ltabor@ironparadise.dev.com");
                await EnsureRole(serviceProvider, user3ID, Constants.User);
                await SeedDB(context, user3ID, "Leah", "Tabor", "ltabor@ironparadise.dev.com");
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<User>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new User
                {
                    UserName = UserName,
                    EmailConfirmed = true //auto-confirm email since smtp server is not available for dev work
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                            string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role)) //This checks to see if a role exists and then creates it if it does not
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<User>>();

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            IR = await userManager.AddToRoleAsync(user, role); //assign the user to a role. The method 'RemoveFromRoleAsync' would remove the user from the role

            return IR;
        }

        private static async Task<IdentityResult> RemoveUserFromRole(IServiceProvider serviceProvider,
                                                                    string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role)) //This checks to see if a role exists and then creates it if it does not
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<User>>();

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            IR = await userManager.RemoveFromRoleAsync(user, role);

            return IR;
        }
        public static async Task SeedDB(ApplicationDbContext context, string ID, string first, string last, string email)
        {
            var user = await context.Users.FindAsync(ID);

            if (user != null)
            {
                user.FirstName = first;
                user.LastName = last;
                user.Email = email;
                user.NormalizedEmail = email.ToUpper();

                await context.SaveChangesAsync();
            }
        }
    }
}