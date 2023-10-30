using AutoMapper;
using eCourier.Data;
using eCourier.Dto;
using eCourier.Dto.CriteriaDto;
using eCourier.Helper;
using eCourier.Models;
using eCourier.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace eCourier.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CustomerRepository(ApplicationDbContext dbContext
            , IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Customer> CreateOrUpdateCustomerAsync(CustomerDto customerDto)
        {
            if (customerDto is null)
            {
                return null;
            }

            var customer =await _dbContext.Customers
                                .FirstOrDefaultAsync(c => c.Phone == customerDto.Phone);

            if (customer is not null)
            {
                _mapper.Map(customerDto, customer);

                _dbContext.Customers.Update(customer);
                if(await _dbContext.SaveChangesAsync() > 0)
                {
                    return customer;
                }
            }
            customer = _mapper.Map<Customer>(customerDto);
            await _dbContext.Customers.AddAsync(customer);
            if(await _dbContext.SaveChangesAsync() > 0)
            {
                return customer;
            }
            return null;
        }

        public async Task<PagedList<Customer>> GetCustomersByCriteriaAsync(CustomerCriteriaDto criteriaDto)
        {
            var query = _dbContext.Customers
               .AsQueryable()
               .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(criteriaDto.CustomerName))
            {
                query = query.Where(o => o.Name.ToLower() == criteriaDto.CustomerName.Trim().ToLower());
            }

            return await PagedList<Customer>.CreateAsync(query, criteriaDto.PageSize, criteriaDto.PageNumber);
        }

        public async Task<bool> TryGetCustomerByIdAsync(int id)
        {
            return await Task.FromResult(true);
        }
    }
}
