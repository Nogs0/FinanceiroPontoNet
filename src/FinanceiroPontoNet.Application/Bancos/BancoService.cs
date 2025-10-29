using FinanceiroPontoNet.Application.Bancos.Dtos;
using FinanceiroPontoNet.Domain.Bancos;

namespace FinanceiroPontoNet.Application.Bancos
{
    public class BancoService : IBancoService
    {
        private readonly IBancoRepository _repository;

        public BancoService(IBancoRepository repository)
        {
            _repository = repository;
        }

        public Task<BancoDto> CreateAsync(BancoDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BancoDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BancoDto> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BancoDto> UpdateAsync(BancoDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
