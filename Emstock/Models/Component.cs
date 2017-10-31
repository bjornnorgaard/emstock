using System.ComponentModel;
using Models.Enums;

namespace Models
{
    public class Component
    {
        public long Id { get; set; }
        public int Number { get; set; }
        [DisplayName("Serial no.")]
        public string SerialNo { get; set; }
        public ComponentStatus Status { get; set; }
        [DisplayName("Admin Comment")]
        public string AdminComment { get; set; }
        [DisplayName("User Comment")]
        public string UserComment { get; set; }
        [DisplayName("Current loan")]
        public long? CurrentLoanInformationId { get; set; }

        public int TypeId { get; set; }
        public Type Type { get; set; }
    }
}
