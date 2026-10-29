namespace NexaWorks.Models.Entities
{
    public class ProductVersion
    {
        public int Id { get; set; }
        public string VersionNumber { get; set; } = string.Empty;

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public ICollection<ProductVersionOs> ProductVersionOs { get; set; } = new HashSet<ProductVersionOs>();
    }
}
