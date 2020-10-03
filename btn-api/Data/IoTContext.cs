using btn_api.Models;
using Microsoft.EntityFrameworkCore;

namespace btn_api.Data
{
    public class IoTContext : DbContext
    {
        public IoTContext(DbContextOptions<IoTContext> options) : base(options) { }
        public DbSet<Mixing> Mixing { get; set; }
        public DbSet<RawData> RawData { get; set; }
        public DbSet<CycleTime> CycleTimes { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mixing>().HasKey(x => x.ID);// um
            modelBuilder.Entity<CycleTime>().HasNoKey();// um
        }

    }
}