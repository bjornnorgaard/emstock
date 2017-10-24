using System.Linq;

namespace DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(EmstockContext context)
        {
            context.Database.EnsureCreated();

            if (context.Categories.Any()) return;
            // TODO Seed categories
            context.SaveChanges();

            if (context.Types.Any()) return;
            // TODO Seed types
            context.SaveChanges();
        }
    }
}