using System.Collections.Generic;
using System.ComponentModel;

namespace Models
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }

        [DisplayName("Types")]
        public List<CategoryType> CategoryTypes { get; set; }
    }
}
