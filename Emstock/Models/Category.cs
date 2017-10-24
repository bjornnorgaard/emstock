using System.Collections.Generic;

namespace Models
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public List<CategoryType> CategoryTypes { get; set; }
    }
}
