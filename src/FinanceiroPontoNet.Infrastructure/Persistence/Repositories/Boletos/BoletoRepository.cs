using FinanceiroPontoNet.Domain.Boletos;
using FinanceiroPontoNet.Domain.Boletos;

namespace FinanceiroPontoNet.Infrastructure.Persistence.Repositories
{
    public class BoletoRepository : Repository<Boleto>, IBoletoRepository
    {
        public BoletoRepository(AppDbContext context)
            : base(context) { }
    }
}
