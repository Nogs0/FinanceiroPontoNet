using FinanceiroPontoNet.Application.Boletos.Dtos;
using FinanceiroPontoNet.Domain.Boletos;
using FinanceiroPontoNet.Domain.Shared.Exceptions;
using FinanceiroPontoNet.Domain.Shared.UnitOfWork;

namespace FinanceiroPontoNet.Application.Boletos
{
    public class BoletoService : IBoletoService
    {
        private readonly IBoletoRepository _repository;
        private readonly IUnitOfWork _uow;

        public BoletoService(IBoletoRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<BoletoDto> CreateAsync(CreateBoletoDto dto)
        {
            var banco = new Boleto(
                dto.NomeDoPagador,
                dto.DocumentoDoPagador,
                dto.NomeDoBeneficiario,
                dto.Valor,
                dto.DataDeVencimento,
                dto.BancoId
            );

            await _repository.CreateAsync(banco);
            await _uow.SaveChangesAsync();
            return new BoletoDto(banco);
        }

        public async Task<List<BoletoDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(e => new BoletoDto(e)).ToList();
        }

        public async Task<BoletoDto> GetAsync(Guid id)
        {
            var entity =
                await _repository.GetByIdAsync(id) ?? throw new NotFoundException("Boleto", id);

            return new BoletoDto(entity);
        }

        public async Task UpdateAsync(BoletoDto dto)
        {
            var entity =
                await _repository.GetByIdAsync(dto.Id)
                ?? throw new NotFoundException("Boleto", dto.Id);

            entity.Atualizar(
                dto.NomeDoPagador,
                dto.NomeDoBeneficiario,
                dto.DocumentoDoPagador,
                dto.DataDeVencimento,
                dto.BancoId,
                dto.Valor
            );

            _repository.Update(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity =
                await _repository.GetByIdAsync(id) ?? throw new NotFoundException("Boleto", id);
            _repository.Delete(entity);

            await _uow.SaveChangesAsync();
        }
    }
}
