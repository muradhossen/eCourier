using eCourier.Dto;
using eCourier.Dto.CriteriaDto;
using eCourier.Helper;
using eCourier.Models;

namespace eCourier.Repositories.Abstraction
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(OrderDto order);
        Task<PagedList<Order>> GetOrdersByCriteriaAsync(OrderCriteriaDto criteriaDto);
        Task<OrderStatusDto> GetOrderStatusAsync(string consigmentNumber);
        Task<OrderDto> GetOrderDetails(int id);
        Task<bool> UpdateOrderAsync(Order order);
    }
}
