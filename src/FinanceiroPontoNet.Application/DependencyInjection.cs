using FinanceiroPontoNet.Application.Bancos;
using FinanceiroPontoNet.Application.Boletos;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceiroPontoNet.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IBoletoService, BoletoService>();
            services.AddScoped<IBancoService, BancoService>();

            return services;
        }
    }
}
