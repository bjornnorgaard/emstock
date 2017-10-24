using System.ComponentModel;
using Models.Enums;

namespace Models
{
    public class Component
    {
        public long Id { get; set; }
        [DisplayName("Number")]
        public int Number { get; set; }
        [DisplayName("Serial no.")]
        public string SerialNo { get; set; }
        public ComponentStatus Status { get; set; }
        [DisplayName("Comment (adm)")]
        public string AdminComment { get; set; }
        [DisplayName("Comment (user)")]
        public string UserComment { get; set; }
        [DisplayName("Current loan")]
        public long? CurrentLoanInformationId { get; set; }

        public int ComponentTypeId { get; set; }
        public Type Type { get; set; }
    }
}
