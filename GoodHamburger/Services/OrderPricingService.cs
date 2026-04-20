using GoodHamburger.Models;
using GoodHamburger.Models.Enum;
using GoodHamburger.Services.Interfaces;

namespace GoodHamburger.Services
{
    public class OrderPricingService : IOrderPricingService
    {
        public void Calculate(Order order)
        {
            var hasSandwich = order.Items.Any(i => i.Item.Category == ItemCategory.Sandwich);
            var hasFries = order.Items.Any(i => i.Item.Category == ItemCategory.Fries);
            var hasDrink = order.Items.Any(i => i.Item.Category == ItemCategory.Drink);

            order.Subtotal = order.Items.Sum(i => i.Item.Price);

            decimal discountPercentage = 0;

            if (hasSandwich && hasFries && hasDrink)
            {
                discountPercentage = 0.20m;
            }
            else if (hasSandwich && hasDrink)
            {
                discountPercentage = 0.15m;
            }
            else if (hasSandwich && hasFries)
            {
                discountPercentage = 0.10m;
            }

            order.Discount = order.Subtotal * discountPercentage;
            order.Total = order.Subtotal - order.Discount;
        }
    }
}
