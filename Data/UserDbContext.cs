using JwtAuthDotNet9_2025.Entities;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthDotNet9_2025.Data
{
    public class UserDbContext(DbContextOptions<UserDbContext>options) : DbContext(options)
    {
        public DbSet<User> Users {  get; set; }
    }
}
