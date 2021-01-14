namespace Frontend.Models
{
    public class ProductImageName
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public int ProductId { get; set; }
        public bool IsDefault { get; set; }
    }
}