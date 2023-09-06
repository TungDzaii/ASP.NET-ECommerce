using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data
{
    public class serverContext : DbContext
    {
        public serverContext(DbContextOptions<serverContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; } = default!;

        public DbSet<Brand> Brand { get; set; } = default!;

        public DbSet<server.Models.Address> Address { get; set; } = default!;

        public DbSet<Order> Order { get; set; } = default!;

        public DbSet<Order_detail> Order_detail { get; set; } = default!;

        public DbSet<server.Models.User> User { get; set; } = default!;
    }
}
