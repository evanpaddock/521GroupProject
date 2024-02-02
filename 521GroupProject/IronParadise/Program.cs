using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using IronParadise.Data;
using IronParadise.Models;
using Microsoft.AspNetCore.Mvc.ViewEngines;
namespace IronParadise
{

    public class Program
    {
        async public static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
		
			builder.Services.AddControllersWithViews();
			builder.Services.AddRazorPages();

            var config = builder.Configuration;
            builder.Services.AddAuthentication()
                //required install of Microsoft.AspNetCore.Authentication.Google nuget package (version 6.0.24 due to .NET 6)
                //SEE the following URL on how to create the ClientId and ClientSecret - https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-6.0#create-the-google-oauth-20-client-id-and-secret
                .AddGoogle(options =>
                {
                    options.ClientId = config["Authentication:Google:ClientId"];
                    options.ClientSecret = config["Authentication:Google:ClientSecret"];
                })
                //required install of Microsoft.AspNetCore.Authentication.MiscrosoftAccount nuget package (version 6.0.24 due to .NET 6)
                //SEE the following URL on how to create the ClientId and ClientSecret - https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins?view=aspnetcore-6.0
                .AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = config["Authentication:Microsoft:ClientId"];
                    microsoftOptions.ClientSecret = config["Authentication:Microsoft:ClientSecret"];
                });

            builder.Services.AddScoped<UserManager<User>>();
            builder.Services.AddScoped<ApplicationDbContext>();

            builder.Services.AddTransient<GetReports>();
            builder.Services.AddSingleton<GPT>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                var testUserAdminPw = builder.Configuration.GetValue<string>("SeedUserAdminPW");
                var testUserManagerPw = builder.Configuration.GetValue<string>("SeedUserManagerPW");
                var testUserPw = builder.Configuration.GetValue<string>("SeedUserPW");
                await SeedData.Initialize(services, testUserAdminPw, testUserManagerPw, testUserPw);
            }

            app.Run();
        }
    }
}