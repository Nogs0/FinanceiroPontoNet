using FinanceiroPontoNet.Application.Bancos;
using FinanceiroPontoNet.Application.Boletos.Dtos;
using FinanceiroPontoNet.Domain.Bancos;
using FinanceiroPontoNet.Domain.Boletos;
using FinanceiroPontoNet.Domain.Shared.Exceptions;
using FinanceiroPontoNet.Domain.Shared.UnitOfWork;

namespace FinanceiroPontoNet.Application.Boletos
{
    public class BoletoService : IBoletoService
    {
        private readonly IBoletoRepository _repository;
        private readonly IBancoService _bancoService;
        private readonly IUnitOfWork _uow;

        public BoletoService(
            IBoletoRepository repository,
            IBancoService bancoService,
            IUnitOfWork uow
        )
        {
            _repository = repository;
            _bancoService = bancoService;
            _uow = uow;
        }

        public async Task<BoletoDto> CreateAsync(CreateBoletoDto dto)
        {
            var banco = new Boleto(
                dto.NomeDoPagador,
                dto.DocumentoDoPagador,
                dto.NomeDoBeneficiario,
                dto.DocumentoDoBeneficiario,
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

            if (entity.DataDeVencimento.Date < DateTime.Now.Date)
            {
                var banco = await _bancoService.GetAsync(entity.BancoId);
                entity.AdicionarJurosDeVencimento(banco.PercentualDeJuros);
            }

            return new BoletoDto(entity);
        }

        public async Task UpdateAsync(BoletoDto dto)
        {
            var entity =
                await _repository.GetByIdAsync(dto.Id)
                ?? throw new NotFoundException("Boleto", dto.Id);

            entity.Atualizar(
                dto.NomeDoPagador,
                dto.DocumentoDoPagador,
                dto.NomeDoBeneficiario,
                dto.DocumentoDoBeneficiario,
                dto.Valor,
                dto.DataDeVencimento,
                dto.BancoId
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
