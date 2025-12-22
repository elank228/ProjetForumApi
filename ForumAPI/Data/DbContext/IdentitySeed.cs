using ForumAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace ForumApi.Data
{
    public static class IdentitySeed
    {
        public static async Task SeedRolesAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<Role>>();

            string[] roles = { "Admin", "Membre", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new Role
                    {
                        Id = Guid.NewGuid(),
                        Name = role,
                        NormalizedName = role.ToUpper()
                    });
                }
            }
        }
    }
}
