namespace NexaWorks.Models.Entities
{
    public class Os
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<ProductVersionOs> ProductVersionOs { get; set; } = new HashSet<ProductVersionOs>();
    }
}
