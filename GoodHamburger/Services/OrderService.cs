using GoodHamburger.Data;
using GoodHamburger.Models;
using GoodHamburger.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }
    }
}
