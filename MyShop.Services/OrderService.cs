using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Services
{
    
    public class OrderService : IOrderservice
    {
       IRepository<Order> orderRepository;
        public OrderService(IRepository<Order> orderRepsoitory)
        {
            this.orderRepository = orderRepsoitory;
        }

        public void CreateOrder(Order order, List<BasketItemViewModel> basketItems)
        {
            foreach (var item in basketItems)
            {
                order.orderItems.Add(new OrderItem()
                {
                    ProductId=item.Id,
                    Image=item.Image,
                    Price=item.Price,
                    ProductName=item.ProductName,
                    Quantity=item.Quantity
                });

                orderRepository.Insert(order);
                orderRepository.Commit();

            }
        }
    }
}
