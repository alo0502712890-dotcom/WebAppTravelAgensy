using System.Collections.Generic;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebApp.Entity;

namespace WebApp.DB
{
    public class AgencyDBContext : DbContext
    {
        public AgencyDBContext(DbContextOptions<AgencyDBContext> options)
            : base(options)
        {
            // Database.EnsureDeleted();
            // Database.EnsureCreated();
        }

        public DbSet<Option> Options { get; set; }
        public DbSet<Navigate> Navigates { get; set; }
        public DbSet<ClientMessage> ClientMessages { get; set; }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostTags> PostTags { get; set; }
        public DbSet<PostCategories> PostCategories { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
