using AutoMapper;
using eCourier.Dto;
using eCourier.Models;

namespace eCourier.Helper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<CustomerDto, Customer>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<Order, OrderDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();
        }
    }
}
