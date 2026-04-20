namespace GoodHamburger.Dtos
{
    public class OrderItemResponseDto
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}