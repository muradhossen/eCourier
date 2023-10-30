using eCourier.Repositories.Abstraction;
using eCourier.Repository;

namespace eCourier.Extention
{
    public static class DependencyContainer
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
        }
    }
}
