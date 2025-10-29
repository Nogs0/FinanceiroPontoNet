using FinanceiroPontoNet.Application.Boletos.Dtos;
using FinanceiroPontoNet.Domain.Boletos;

namespace FinanceiroPontoNet.Application.Boletos
{
    public class BoletoService : IBoletoService
    {
        private readonly IBoletoRepository _repository;

        public BoletoService(IBoletoRepository repository)
        {
            _repository = repository;
        }

        public Task<BoletoDto> CreateAsync(BoletoDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BoletoDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BoletoDto> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BoletoDto> UpdateAsync(BoletoDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
