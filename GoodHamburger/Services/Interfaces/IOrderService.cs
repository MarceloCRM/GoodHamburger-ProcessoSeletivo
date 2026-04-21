using GoodHamburger.Dtos;

namespace GoodHamburger.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderResponseDto>> GetAllAsync();
        Task<OrderResponseDto> GetByIdAsync(int id);
        Task<OrderResponseDto> AddAsync(CreateOrderDto dto);
        Task<OrderResponseDto> UpdateAsync(int id, CreateOrderDto dto);
        Task<OrderResponseDto> DeleteAsync(int id);
    }
}