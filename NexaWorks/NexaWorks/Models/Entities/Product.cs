namespace NexaWorks.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<ProductVersion> ProductVersions { get; set; } = new HashSet<ProductVersion>();
        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
