using Microsoft.EntityFrameworkCore;

namespace OnlinefoodDeliverySystem.Models
{
    public class FoodOrderingDbContext : DbContext
    {
        public FoodOrderingDbContext(DbContextOptions<FoodOrderingDbContext> options) : base(options)
        {
        }

        public DbSet<FoodData> FoodOrders { get; set; }
    }
}
