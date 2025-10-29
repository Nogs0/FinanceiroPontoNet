using FinanceiroPontoNet.Domain.Shared;

namespace FinanceiroPontoNet.Domain.Bancos
{
    public class Banco : FullAuditedEntity, ISoftDelete
    {
        public string Nome { get; set; } = "";
        public string Codigo { get; set; } = "";
        public decimal PercentualDeJuros { get; set; }
        public DateTime? DeletedAt { get; set; }

        private Banco() { }
        public Banco(string nome, string codigo, decimal percentualDeJuros)
        {
            Nome = nome;
            Codigo = codigo;
            PercentualDeJuros = percentualDeJuros;
        }

        public void Atualizar(string nome, string codigo, decimal percentualDeJuros)
        {
            Nome = nome;
            Codigo = codigo;
            PercentualDeJuros = percentualDeJuros;
        }
    }
}
