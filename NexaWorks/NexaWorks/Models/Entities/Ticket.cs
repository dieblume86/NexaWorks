using NexaWorks.Models.Enums;

namespace NexaWorks.Models.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset? ResolutionDate { get; set; }

        public ETicketStatus Status { get; set; } = ETicketStatus.InProgress;

        public string Issue { get; set; } = string.Empty;
        public string? Resolution { get; set; }


        public int ProductVersionOsId { get; set; }
        public ProductVersionOs? ProductVersionOs { get; set; }
    }
}
