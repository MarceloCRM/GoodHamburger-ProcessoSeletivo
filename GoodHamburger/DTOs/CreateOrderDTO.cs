namespace GoodHamburger.Dtos
{
    public class CreateOrderDto
    {
        public List<int> ItemIds { get; set; } = new();
    }
}