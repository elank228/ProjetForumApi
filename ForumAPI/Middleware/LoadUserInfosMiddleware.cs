using ForumApi.Exeptions;
using ForumAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ForumApi.Middlewares
{
    public class LoadUserInfosMiddleware
    {
        public readonly RequestDelegate _next;

        public LoadUserInfosMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<User> userManager)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                Guid userId = Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier));

                User? user = await userManager.Users
                    .Where(u => u.Id == userId)
                    //.Include(u => u.Organization)
                    .FirstOrDefaultAsync();

                var iRoles = await userManager.GetRolesAsync(user);

                List<string> roles = iRoles.ToList();

                if (user == null)
                {
                    throw new ForbiddenException("User does not exist.");
                }

                context.Items["roles"] = roles;
                context.Items["user"] = user;
            }

            await _next(context);
        }
    }
}