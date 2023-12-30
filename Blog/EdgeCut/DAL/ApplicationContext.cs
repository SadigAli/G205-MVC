using EdgeCut.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EdgeCut.DAL
{
    public class ApplicationContext : IdentityDbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {            
        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Furniture> Furnitures { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
