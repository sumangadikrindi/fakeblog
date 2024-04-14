using FakeBlog.Blogs.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FakeBlog.Blogs.Api.Data;

public class BlogsDbContext : DbContext
{
    public BlogsDbContext(DbContextOptions<BlogsDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    internal DbSet<Blog> Blogs { get; set; }
}
