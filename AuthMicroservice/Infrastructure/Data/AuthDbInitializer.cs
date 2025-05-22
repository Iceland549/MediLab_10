using Microsoft.AspNetCore.Identity;
using AuthMicroservice.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AuthMicroservice.Infrastructure.Data
{
    public static class AuthDbInitializer
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var userName = Environment.GetEnvironmentVariable("ADMIN_USERNAME") ?? "admin";
            var password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "Admin123!";

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                var newUser = new User { UserName = userName, Email = $"{userName}@demo.fr" };
                await userManager.CreateAsync(newUser, password);
            }
        }
    }
}