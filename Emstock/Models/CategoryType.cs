namespace Models
{
    public class CategoryType
    {
        public long TypeId { get; set; }
        public Type Type { get; set; }

        public long CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
