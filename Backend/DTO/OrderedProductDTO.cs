namespace Backend.DTO
{
    public class OrderedProductDTO
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}