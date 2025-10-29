using FinanceiroPontoNet.Domain.Bancos;
using FinanceiroPontoNet.Domain.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FinanceiroPontoNet.Infrastructure.Persistence.Repositories
{
    public class BancoRepository : Repository<Banco>, IBancoRepository
    {
        public BancoRepository(AppDbContext context)
            : base(context) { }

        public async Task<Banco?> GetByCodigoAsync(string codigo)
        {
            var banco = await _dbSet.FirstOrDefaultAsync(x => x.Codigo == codigo);

            return banco;
        }
    }
}
