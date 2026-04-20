using GoodHamburger.Models;

namespace GoodHamburger.Services.Interfaces
{
    public interface IOrderPricingService
    {
        void Calculate(Order order);
    }
}
