using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class EmstockContext : DbContext
    {
        public DbSet<Models.Component> Components { get; set; }
        public DbSet<Models.Image> Images { get; set; }
        public DbSet<Models.Type> Types { get; set; }
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.CategoryType> CategoryType { get; set; }

        public EmstockContext(DbContextOptions<EmstockContext> options) : base(options) { }

        public EmstockContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.CategoryType>()
                .HasKey(c => new { c.CategoryId, c.TypeId });

            modelBuilder.Entity<Models.CategoryType>()
                .HasOne(ct => ct.Category)
                .WithMany(c => c.CategoryTypes)
                .HasForeignKey(k => k.CategoryId);

            modelBuilder.Entity<Models.CategoryType>()
                .HasOne(ct => ct.Type)
                .WithMany(c => c.CategoryTypes)
                .HasForeignKey(k => k.TypeId);
        }
    }
}
