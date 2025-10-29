using FinanceiroPontoNet.Domain.Bancos;
using FinanceiroPontoNet.Domain.Boletos;
using FinanceiroPontoNet.Infrastructure.Persistence;
using FinanceiroPontoNet.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceiroPontoNet.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this ServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql());

        services.AddScoped<IBancoRepository, BancoRepository>();
        services.AddScoped<IBoletoRepository, BoletoRepository>();

        return services;
    }
}
