using FinanceiroPontoNet.Application.Shared;
using FinanceiroPontoNet.Domain.Boletos;

namespace FinanceiroPontoNet.Application.Boletos.Dtos
{
    public class BoletoDto : BaseEntityDto
    {
        public string NomeDoPagador { get; set; } = "";
        public string DocumentoDoPagador { get; set; } = "";
        public string NomeDoBeneficiario { get; set; } = "";
        public decimal Valor { get; set; }
        public DateTime DataDeVencimento { get; set; }
        public Guid BancoId { get; set; }

        public BoletoDto() { }

        public BoletoDto(Boleto entity)
        {
            Id = entity.Id;
            NomeDoPagador = entity.NomeDoPagador;
            DocumentoDoPagador = entity.DocumentoDoPagador;
            NomeDoBeneficiario = entity.NomeDoBeneficiario;
            Valor = entity.Valor;
            DataDeVencimento = entity.DataDeVencimento;
            BancoId = entity.BancoId;
        }
    }
}
