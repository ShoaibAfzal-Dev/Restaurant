using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace WEB_Api.Models
{
    public class MyDbContext: IdentityDbContext
    {
        public DbSet<Category> Category { get; set; }
        /*public DbSet<Coupon> Coupon { get; set; }*/
        public DbSet<Products> Products { get; set; }
        public DbSet<Order_Types> Order_types { get; set; }
        public DbSet<Order_SubTypes> Order_SubTypes { get; set; }
        public DbSet<User_details> User_details { get; set; }
        public DbSet<AddToCart> addToCarts { get; set; }
        public DbSet<AddToCart_Sub> addToCart_Subs { get; set; }
        public DbSet<Order_Details> Order_Details { get; set; }
        public DbSet<Order_SubDetails> Order_SubDetails { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        { }
        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>()
                .HasOne(p => p.Category)
                .WithMany(b => b.product)
                .HasForeignKey(p => p.Categoryid);
            base.OnModelCreating(modelBuilder);
        }*/
    }
}
