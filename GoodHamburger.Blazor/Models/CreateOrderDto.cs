namespace GoodHamburger.Blazor.Models
{
    public class CreateOrderDto
    {
        public List<int> ItemIds { get; set; } = new();
    }
}
