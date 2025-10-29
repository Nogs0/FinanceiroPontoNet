using FinanceiroPontoNet.Application.Shared;
using FinanceiroPontoNet.Domain.Bancos;
using FinanceiroPontoNet.Domain.Shared;

namespace FinanceiroPontoNet.Application.Bancos.Dtos
{
    public class BancoDto : BaseEntityDto
    {
        public string Nome { get; set; } = "";
        public string Codigo { get; set; } = "";
        public decimal PercentualDeJuros { get; set; }

        public BancoDto(Banco entity)
        {
            Id = entity.Id;
            Nome = entity.Nome;
            Codigo = entity.Codigo;
            PercentualDeJuros = entity.PercentualDeJuros;
        }
    }
}
