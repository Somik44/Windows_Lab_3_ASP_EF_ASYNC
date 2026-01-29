using Lab_3_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab_3_Backend.Data
{
    public class CarDbContext : DbContext
    {
        public CarDbContext(DbContextOptions<CarDbContext> options) : base(options) { }

        public CarDbContext() : base()
        {
        }

        public DbSet<Cars> Cars { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Truck> Trucks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cars>()
            .HasDiscriminator<string>("Type")
            .HasValue<Passenger>("Легковой") 
            .HasValue<Truck>("Грузовой");
        }
    }
}
