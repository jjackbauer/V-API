using Microsoft.EntityFrameworkCore;
using VolvoApi.Model;

namespace VolvoApi.Data.Context
{
    public class TruckDbContext :DbContext
    {
        public DbSet<Truck> Trucks { get; set; }
        public TruckDbContext(DbContextOptions<TruckDbContext> opt) : base(opt)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Truck>
            (
                builder => 
                {
                    builder.HasKey(x => x.Id);

                    builder.Property(x => x.Name).IsRequired();
                    builder.Property(x => x.Description).IsRequired();
                    builder.Property(x => x.Model).IsRequired();
                    builder.Property(x => x.ManufacturingYear).IsRequired();
                    builder.Property(x => x.ModelYear).IsRequired();
                    builder.Property(x => x.CreatedAt).IsRequired();
                    builder.Property(x => x.ModifiedAt).IsRequired();

                }


            );
        }
    }
}
