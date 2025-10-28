using System.ComponentModel.DataAnnotations;
using FinanceiroPontonet.Domain.Bancos;
using FinanceiroPontonet.Domain.Shared;

namespace FinanceiroPontonet.Domain.Boletos
{
    public class Boleto : FullAuditedEntity, ISoftDelete
    {
        [Required]
        public string NomeDoPagador { get; set; } = "";

        [Required]
        public string DocumentoDoPagador { get; set; } = "";

        [Required]
        public string NomeDoBeneficiario { get; set; } = "";

        public decimal Valor { get; set; }
        public DateTime DataDeVencimento { get; set; }
        public Guid BancoId { get; set; }
        public Banco? Banco { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
