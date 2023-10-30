using AutoMapper;
using eCourier.Data;
using eCourier.Dto;
using eCourier.Dto.CriteriaDto;
using eCourier.Helper;
using eCourier.Models;
using eCourier.Repositories.Abstraction;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace eCourier.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderRepository(ApplicationDbContext dbContext
            , IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Order> CreateOrderAsync(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto, opt => opt.AfterMap(async (src, des) =>
              {
                  des.RecipientId = orderDto.RecipientId;
                  des.AppUserId = orderDto.AppUserId;
                  des.ConsignmentNumber = await GenerateConsigmentNumber();
              }));


            await _dbContext.Orders.AddAsync(order);
            var isSaved = await _dbContext.SaveChangesAsync() > 0;
            if (isSaved)
            {
                return order;
            }
            return null;
        }

        public async Task<PagedList<Order>> GetOrdersByCriteriaAsync(OrderCriteriaDto criteriaDto)
        {
            var query = _dbContext.Orders
                .AsQueryable()
                .AsNoTracking();

            if (criteriaDto.AppUserId is not null && criteriaDto.AppUserId > 0)
            {
                query = query.Where(c => c.AppUserId == criteriaDto.AppUserId);
            }

            if (!string.IsNullOrWhiteSpace(criteriaDto.ConsignmentNumber))
            {
                query = query.Where(o => o.ConsignmentNumber == criteriaDto.ConsignmentNumber.Trim());
            }

            return await PagedList<Order>.CreateAsync(query, criteriaDto.PageSize, criteriaDto.PageNumber);
        }

        private async Task<string> GenerateConsigmentNumber()
        {
            var randomNumber = Random.Shared.Next(100, 1000);


            var maxOrderId = await _dbContext.Orders
                .OrderByDescending(c => c.Id)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            //.MaxAsync(o => o.Id);

            return $"ODR-{randomNumber}-{maxOrderId + 1}";

        }

        public async Task<OrderStatusDto> GetOrderStatusAsync(string consigmentNumber)
        {
            if (string.IsNullOrWhiteSpace(consigmentNumber))
            {
                throw new Exception("Please provide a consigment number for order details!");
            }

            return await _dbContext.Orders
                 .Where(o => o.ConsignmentNumber == consigmentNumber)
                 .Select(o => new OrderStatusDto
                 {
                     SenderName = o.AppUser.UserName,
                     RecipientName = o.Recipient.Name,
                     ReachDate = o.ReachDate,
                     Status = o.Status,
                     DueAmount = o.DueAmount
                 })
             .FirstOrDefaultAsync();
        }

        public async Task<OrderDto> GetOrderDetails(int id)
        {
            var order = await _dbContext.Orders
                 .Include(c => c.AppUser)
                 .Include(c => c.Recipient)
                 .Include(c => c.Products)
                 .AsNoTracking()
                 .FirstOrDefaultAsync(c => c.Id == id);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            try
            {
                _dbContext.Orders.Update(order);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await OrderExists(order.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }
        private async Task<bool> OrderExists(int id)
        {
            return await _dbContext.Orders.AnyAsync(e => e.Id == id);
        }
    }
}
