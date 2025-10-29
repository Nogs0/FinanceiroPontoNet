using FinanceiroPontoNet.Application.Bancos.Dtos;
using FinanceiroPontoNet.Application.Shared;

namespace FinanceiroPontoNet.Application.Bancos
{
    public interface IBancoService : IServiceCrud<BancoDto, CreateBancoDto>
    {
        Task<BancoDto> GetByCodigoAsync(string codigo);
    }
}
