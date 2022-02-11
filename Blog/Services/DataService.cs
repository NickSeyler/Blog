using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services
{
    public class DataService
    {
        readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        public DataService(ApplicationDbContext context, 
                           RoleManager<IdentityRole> roleManager,
                           UserManager<BlogUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SetupDBAsync()
        {
            //Run the Migrations async
            await _context.Database.MigrateAsync();

            //Seed the database
            await SeedRolesAsync();

            await SeedUsersAsync();
            
        }

        private async Task SeedRolesAsync()
        {
            if (_roleManager.Roles.Count() == 0)
            {
                await _roleManager.CreateAsync(new IdentityRole("Administrator"));
                await _roleManager.CreateAsync(new IdentityRole("Moderator"));
            }
        }

        private async Task SeedUsersAsync()
        {
            BlogUser adminUser = new()
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                FirstName = "Nick",
                LastName = "Seyler",
                DisplayName = "Nick Seyler",
                PhoneNumber = "555 555 5555",
                EmailConfirmed = true
            };

            try
            {
                if (await _userManager.FindByEmailAsync(adminUser.Email) != null)
                {
                    await _userManager.CreateAsync(adminUser, "AdminBlogPassword1*");
                    await _userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }
            catch (Exception)
            {
                throw;
            }

            BlogUser modUser = new()
            {
                UserName = "mod@example.com",
                Email = "mod@example.com",
                FirstName = "Mod",
                LastName = "Example",
                DisplayName = "Mod Example",
                PhoneNumber = "555 555 5555",
                EmailConfirmed = true
            };

            try
            {
                if (await _userManager.FindByEmailAsync(modUser.Email) != null)
                {
                    await _userManager.CreateAsync(modUser, "ModBlogPassword1*");
                    await _userManager.AddToRoleAsync(modUser, "Moderator");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
