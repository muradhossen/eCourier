using eCourier.Dto;
using eCourier.Dto.CriteriaDto;
using eCourier.Helper;
using eCourier.Models;

namespace eCourier.Repositories.Abstraction
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateOrUpdateCustomerAsync(CustomerDto customer);
        Task<PagedList<Customer>> GetCustomersByCriteriaAsync(CustomerCriteriaDto criteriaDto);
        Task<bool> TryGetCustomerByIdAsync(int id);
    }
}
