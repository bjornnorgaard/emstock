using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class EmstockContext : DbContext
    {
        public EmstockContext(DbContextOptions<EmstockContext> options) : base(options) { }

        public EmstockContext() { }

        public DbSet<Models.Component> Components { get; set; }
    }
}
