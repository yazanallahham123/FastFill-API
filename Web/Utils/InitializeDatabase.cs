
using FastFill_API.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Web.Utils
{
    public class InitializeDatabase
    {
        public static async Task Run(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<FastFillDBContext>();
                var securityService = serviceScope.ServiceProvider.GetRequiredService<SecurityServices>();
                applicationDbContext.Database.Migrate();

                if (!applicationDbContext.Users.Any(u => u.RoleId == (int) RoleType.Admin))
                {
                    User user = new User();
                    user.FirstName = "Admin";
                    user.LastName = "Admin";
                    user.Username = "Admin";
                    user.MobileNumber = "Admin";
                    user.RoleId = (int) RoleType.Admin;
                    user.Password = "admin1234";
                    var hashSalt = securityService.EncryptPassword(user.Password);
                    user.Password = hashSalt.Hash;
                    user.StoredSalt = hashSalt.Salt;
                    user.Token = hashSalt.Hash;

                    try
                    {
                        applicationDbContext.Users.Add(user);
                        await applicationDbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
