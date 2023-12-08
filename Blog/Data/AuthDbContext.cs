using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            var superAdminRoleId = "68ad896f-03d5-467f-8d65-368f01ff4c59";
            var adminRoleId = "0c72154f-290d-4b41-80ae-ee0521e8344a";
            var userRoleId = "5c97a74e-9bfd-4434-8fd7-e38dda89e4b6";

            // Seed Roles (User, Admin, UserAdmin) 
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }
            };


            // save roles inside the database 
            builder.Entity<IdentityRole>().HasData(roles);


            // Seed SuperAdminUser 

            var superAdminId = "70fd5bc2-1ca1-4bae-b619-95c3d2f7eb60";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@company.com",
                Email = "superadmin@company.com",
                NormalizedEmail = "superadmin@company.com".ToUpper(),
                NormalizedUserName = "superadmin@company.com".ToUpper(),
                Id = superAdminId
            };

            // store hashed password
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "superadmin@123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);


            // Add All roles to SuperAdminUser 

            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
