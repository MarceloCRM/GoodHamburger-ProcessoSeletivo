using GoodHamburger.Models;

namespace GoodHamburger.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllAsync();
    }
}
