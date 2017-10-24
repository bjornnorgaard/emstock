namespace Models
{
    public class Component
    {
        public long Id { get; set; }
        public int Number { get; set; }
        public string SerialNo { get; set; }
        public ComponentStatus Status { get; set; }
        public string AdminComment { get; set; }
        public string UserComment { get; set; }
        public long? CurrentLoanInformationId { get; set; }
    }
}
