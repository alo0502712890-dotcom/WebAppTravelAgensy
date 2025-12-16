using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Entity;


namespace WebApp.DB
{
    public class AgencyDBContext
            : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AgencyDBContext(DbContextOptions<AgencyDBContext> options)
            : base(options)
        {
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
    }
}
