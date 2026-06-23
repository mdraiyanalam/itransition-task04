using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using task04UserManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace task04UserManagement.Middleware
{
    public class BlockedUserMiddleware
    {
        private readonly RequestDelegate _next;

        public BlockedUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
        {
            if (context.Request.Path.StartsWithSegments("/api/auth"))
            {
                await _next(context);
                return;
            }

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                var user = await dbContext.Users.FindAsync(userId);
                if (user == null || user.Status == "Blocked")
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Your account is blocked or deleted.");
                    return;
                }
            }

            await _next(context);
        }
    }
}