using GoodHamburger.Data;
using GoodHamburger.Dtos;
using GoodHamburger.Models;
using GoodHamburger.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IOrderPricingService _orderPricingService;

        public OrderService(AppDbContext context, IOrderPricingService orderPricingService)
        {
            _context = context;
            _orderPricingService = orderPricingService;
        }

        public async Task<List<OrderResponseDto>> GetAllAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Item)
                .ToListAsync();

            return orders.Select(MapToResponseDto).ToList();
        }

        public async Task<OrderResponseDto> GetByIdAsync(int id)
        {
            var order = await _context.Orders.Include(o => o.Items).ThenInclude(oi => oi.Item).FirstOrDefaultAsync(o => o.Id == id);
            if (order is null)
            {
                throw new Exception("Pedido não encontrado.");
            }

            _orderPricingService.Calculate(order);

            return MapToResponseDto(order);
        }

        public async Task<OrderResponseDto> AddAsync(CreateOrderDto dto)
        {
            if (dto == null || dto.ItemIds == null || !dto.ItemIds.Any())
                throw new Exception("O pedido deve conter ao menos um item.");

            var itemsFromDb = await _context.Items
                .Where(i => dto.ItemIds.Contains(i.Id))
                .ToListAsync();

            if (itemsFromDb.Count != dto.ItemIds.Count)
                throw new Exception("Um ou mais itens informados não existem no cardápio.");

            var hasDuplicateCategory = itemsFromDb
                .GroupBy(i => i.Category)
                .Any(g => g.Count() > 1);

            if (hasDuplicateCategory)
                throw new Exception("O pedido pode conter apenas um item de cada categoria.");

            var order = new Order
            {
                Items = itemsFromDb.Select(item => new OrderItem
                {
                    ItemId = item.Id,
                    Item = item
                }).ToList()
            };

            _orderPricingService.Calculate(order);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return MapToResponseDto(order);
        }
        public async Task<OrderResponseDto> UpdateAsync(int id, CreateOrderDto dto)
        {

            var order = await _context.Orders.Include(oi => oi.Items).ThenInclude(ol => ol.Item).FirstOrDefaultAsync(o => o.Id == id);


            if (order == null)
                throw new Exception("Pedido não encontrado.");

            if (dto.ItemIds == null || !dto.ItemIds.Any())
                throw new Exception("O pedido deve conter ao menos um item.");

            var itemsFromDb = await _context.Items
                .Where(i => dto.ItemIds.Contains(i.Id))
                .ToListAsync();

            if (itemsFromDb.Count != dto.ItemIds.Count)
                throw new Exception("Um ou mais itens não existem.");

            var hasDuplicateCategory = itemsFromDb
                .GroupBy(i => i.Category)
                .Any(g => g.Count() > 1);

            if (hasDuplicateCategory)
                throw new Exception("Apenas um item por categoria é permitido.");


            order.Items = itemsFromDb.Select(item => new OrderItem
            {
                ItemId = item.Id,
                Item = item
            }).ToList();

            _orderPricingService.Calculate(order);

            await _context.SaveChangesAsync();

            return MapToResponseDto(order);
        }

        private static OrderResponseDto MapToResponseDto(Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                Subtotal = order.Subtotal,
                Discount = order.Discount,
                Total = order.Total,
                Items = order.Items.Select(oi => new OrderItemResponseDto
                {
                    ItemId = oi.ItemId,
                    Name = oi.Item?.Name ?? string.Empty,
                    Price = oi.Item?.Price ?? 0,
                    Category = oi.Item?.Category.ToString() ?? string.Empty
                }).ToList()
            };
        }
    }
}