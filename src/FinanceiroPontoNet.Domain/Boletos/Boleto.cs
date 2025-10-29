using System.ComponentModel.DataAnnotations;
using FinanceiroPontoNet.Domain.Bancos;
using FinanceiroPontoNet.Domain.Shared;

namespace FinanceiroPontoNet.Domain.Boletos
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
