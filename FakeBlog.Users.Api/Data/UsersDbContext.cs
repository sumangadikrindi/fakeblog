using FakeBlog.Users.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FakeBlog.Users.Api.Data
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        internal DbSet<User> Users { get; set; }
    }
}