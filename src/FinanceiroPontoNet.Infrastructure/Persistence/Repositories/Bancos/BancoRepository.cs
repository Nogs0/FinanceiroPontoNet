using FinanceiroPontoNet.Domain.Bancos;
using FinanceiroPontoNet.Domain.Bancos;

namespace FinanceiroPontoNet.Infrastructure.Persistence.Repositories
{
    public class BancoRepository : Repository<Banco>, IBancoRepository
    {
        public BancoRepository(AppDbContext context)
            : base(context) { }
    }
}
