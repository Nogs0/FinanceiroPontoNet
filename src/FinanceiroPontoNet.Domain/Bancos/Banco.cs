using FinanceiroPontonet.Domain.Shared;

namespace FinanceiroPontonet.Domain.Bancos
{
    public class Banco : FullAuditedEntity, ISoftDelete
    {
        public string Nome { get; set; } = "";
        public string Codigo { get; set; } = "";
        public decimal PercentualDeJuros { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
