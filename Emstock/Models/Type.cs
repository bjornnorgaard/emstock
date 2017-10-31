using System.Collections.Generic;
using System.ComponentModel;
using Models.Enums;

namespace Models
{
    public class Type
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Location { get; set; }
        public ComponentTypeStatus Status { get; set; }
        public string Datasheet { get; set; }
        [DisplayName("Image URL")]
        public string ImageUrl { get; set; }
        public string Manufacturer { get; set; }
        [DisplayName("Wiki link")]
        public string WikiLink { get; set; }
        [DisplayName("Admin Comment")]
        public string AdminComment { get; set; }

        public List<CategoryType> CategoryTypes { get; set; }
        public List<Component> Components { get; set; }
    }
}
