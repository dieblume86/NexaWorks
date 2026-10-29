namespace NexaWorks.Models.Entities
{
    public class ProductVersionOs
    {
        public int Id { get; set; }

        public int ProductVersionId { get; set; }
        public ProductVersion? ProductVersion { get; set; }

        public int OsId { get; set; }
        public Os? Os { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
