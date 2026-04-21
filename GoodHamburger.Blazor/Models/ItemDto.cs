namespace GoodHamburger.Blazor.Models
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Category { get; set; }
    }
}
