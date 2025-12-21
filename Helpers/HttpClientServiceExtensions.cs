using eCommerceWeb.Services.Implementation;
using eCommerceWeb.Services.Interface;

namespace eCommerceWeb.Helpers
{
    public static class HttpClientServiceExtensions
    {
        public static IServiceCollection AddExternalApiClients(this IServiceCollection services)
        {
            services.AddHttpClient<IProductService, ProductService>();
            services.AddHttpClient<IUserService, UserService>();
            return services;
        }
    }
}

