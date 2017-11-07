using System.Collections.Generic;
using System.ComponentModel;

namespace Models
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }

        [DisplayName("Types of this category")]
        public List<CategoryType> CategoryTypes { get; set; }
    }
}
