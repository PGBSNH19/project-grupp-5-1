namespace Backend.DTO
{
    public class ProductImageDTO
    {
        public int Id { get; set; }

        public string ImageName { get; set; }

        public int ProductId { get; set; }

        public bool IsDefault { get; set; } = false;
    }
}