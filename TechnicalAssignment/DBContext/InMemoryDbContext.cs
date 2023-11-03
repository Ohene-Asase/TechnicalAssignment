using Microsoft.EntityFrameworkCore;
using TechnicalAssignment.Models;

namespace TechnicalAssignment.DBContext
{
    public class InMemoryDbContext:DbContext
    {
       
           
            public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
