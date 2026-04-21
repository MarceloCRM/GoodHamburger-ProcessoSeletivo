namespace GoodHamburger.Blazor.Models
{
    public class CreateOrderDto
    {
        public List<int> ItemsIds { get; set; } = new ();
    }
}
