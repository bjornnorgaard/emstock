using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Enums;

namespace DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(EmstockContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var categories = new List<Category>
            {
                new Category {Name = "Developerboard"},
                new Category {Name = "Microcontroller"},
                new Category {Name = "Resistor"}
            };

            var types = new List<Type>();
            for (var i = 1; i <= 3; i++)
            {
                types.Add(new Type
                {
                    AdminComment = "Im the admin comment",
                    Datasheet = "http://datasheet.com",
                    CategoryTypes = new List<CategoryType>(),
                    ImageUrl = "http://image.com",
                    Info = "information about type",
                    Location = "Highroad 5",
                    Manufacturer = "MakerNoPie",
                    Name = $"Raspberry Pi {i}",
                    Status = ComponentTypeStatus.Available,
                    WikiLink = "http://wikilink.dk"
                });
            }

            context.AddRange(
                new CategoryType { Category = categories[0], Type = types[0] },
                new CategoryType { Category = categories[0], Type = types[1] },
                new CategoryType { Category = categories[1], Type = types[2] },
                new CategoryType { Category = categories[1], Type = types[0] },
                new CategoryType { Category = categories[2], Type = types[1] },
                new CategoryType { Category = categories[2], Type = types[2] }
                );

            if (context.Categories.Any()) return;
            context.AddRange(categories);
            context.SaveChanges();

            if (context.Types.Any()) return;
            context.AddRange(types);
            context.SaveChanges();
        }
    }
}