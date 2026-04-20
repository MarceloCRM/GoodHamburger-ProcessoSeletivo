using GoodHamburger.Dtos;

namespace GoodHamburger.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderResponseDto>> GetAllAsync();
        Task<OrderResponseDto> AddAsync(CreateOrderDto dto);
    }
}