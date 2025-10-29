using FinanceiroPontoNet.Domain.Shared.Repositories;

namespace FinanceiroPontoNet.Domain.Bancos
{
    public interface IBancoRepository : IRepository<Banco>
    {
        Task<Banco?> GetByCodigoAsync(string codigo);
    }
}
