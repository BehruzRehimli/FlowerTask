using FlowersTask.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlowersTask.DAL
{
    public class FlowersDbContext:IdentityDbContext
    {
        public FlowersDbContext(DbContextOptions<FlowersDbContext>opt):base(opt) { }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Catagory> Catagories { get; set; }
        public DbSet<Flower> Flowers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<FlowerCatagory> FlowerCatagory { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlowerCatagory>().HasKey(x => new { x.FlowerId, x.CatagoryId });
            base.OnModelCreating(modelBuilder);
        }

    }
}
