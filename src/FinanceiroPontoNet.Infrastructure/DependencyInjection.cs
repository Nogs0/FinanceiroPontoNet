using FinanceiroPontoNet.Domain.Bancos;
using FinanceiroPontoNet.Domain.Boletos;
using FinanceiroPontoNet.Domain.Shared.UnitOfWork;
using FinanceiroPontoNet.Infrastructure.Persistence;
using FinanceiroPontoNet.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceiroPontoNet.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IUnitOfWork>(provider =>
                provider.GetRequiredService<AppDbContext>()
            );
            services.AddScoped<IBancoRepository, BancoRepository>();
            services.AddScoped<IBoletoRepository, BoletoRepository>();

            return services;
        }
    }
}
