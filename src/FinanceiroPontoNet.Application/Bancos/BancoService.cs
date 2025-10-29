using FinanceiroPontoNet.Application.Bancos.Dtos;
using FinanceiroPontoNet.Domain.Bancos;
using FinanceiroPontoNet.Domain.Shared.Exceptions;
using FinanceiroPontoNet.Domain.Shared.UnitOfWork;

namespace FinanceiroPontoNet.Application.Bancos
{
    public class BancoService : IBancoService
    {
        private readonly IBancoRepository _repository;
        private readonly IUnitOfWork _uow;

        public BancoService(IBancoRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<BancoDto> CreateAsync(CreateBancoDto dto)
        {
            var codigoAlreadyUsed = await _repository.GetByCodigoAsync(dto.Codigo);
            if (codigoAlreadyUsed != null)
                throw new ArgumentException("Código de banco já utilizado.");

            var banco = new Banco(dto.Nome, dto.Codigo, dto.PercentualDeJuros);

            await _repository.CreateAsync(banco);
            await _uow.SaveChangesAsync();
            return new BancoDto(banco);
        }

        public async Task<List<BancoDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(e => new BancoDto(e)).ToList();
        }

        public async Task<BancoDto> GetAsync(Guid id)
        {
            var entity =
                await _repository.GetByIdAsync(id) ?? throw new NotFoundException("Banco", id);

            return new BancoDto(entity);
        }

        public async Task<BancoDto> GetByCodigoAsync(string codigo)
        {
            var banco =
                await _repository.GetByCodigoAsync(codigo)
                ?? throw new NotFoundException("Banco", codigo);
            return new BancoDto(banco);
        }

        public async Task UpdateAsync(BancoDto dto)
        {
            var entity =
                await _repository.GetByIdAsync(dto.Id)
                ?? throw new NotFoundException("Banco", dto.Id);

            entity.Atualizar(dto.Nome, dto.Codigo, dto.PercentualDeJuros);

            _repository.Update(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity =
                await _repository.GetByIdAsync(id) ?? throw new NotFoundException("Banco", id);
            _repository.Delete(entity);

            await _uow.SaveChangesAsync();
        }
    }
}
