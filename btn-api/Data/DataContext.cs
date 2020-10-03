using btn_api.Models;
using Microsoft.EntityFrameworkCore;

namespace btn_api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<BuildingUser> BuildingUsers { get; set; }
        public DbSet<Button> Buttons { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<LineInfo> LineInfos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}