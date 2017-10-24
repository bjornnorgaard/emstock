namespace Models
{
    public class ComponentType
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Location { get; set; }
        public ComponentTypeStatus Status { get; set; }
        public string Datasheet { get; set; }
        public string ImageUrl { get; set; }
        public string Manufacturer { get; set; }
        public string WikiLink { get; set; }
        public string AdminComment { get; set; }
    }
}
