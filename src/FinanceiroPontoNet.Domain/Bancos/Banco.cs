using System.ComponentModel.DataAnnotations;
using FinanceiroPontoNet.Domain.Shared;

namespace FinanceiroPontoNet.Domain.Bancos
{
    public class Banco : FullAuditedEntity, ISoftDelete
    {
        [MaxLength(256)]
        public string Nome { get; set; } = "";

        [MaxLength(64)]
        public string Codigo { get; set; } = "";
        public decimal PercentualDeJuros { get; set; }
        public DateTime? DeletedAt { get; set; }

        private Banco() { }

        public Banco(string nome, string codigo, decimal percentualDeJuros)
        {
            if (string.IsNullOrEmpty(nome))
                throw new ArgumentException("O nome do banco não pode ser vazio");
            Nome = nome;

            if (string.IsNullOrEmpty(codigo))
                throw new ArgumentException("O código do banco não pode ser vazio");
            Codigo = codigo;

            PercentualDeJuros = percentualDeJuros;
        }

        public void Atualizar(string nome, string codigo, decimal percentualDeJuros)
        {
            if (string.IsNullOrEmpty(nome))
                throw new ArgumentException("O nome do banco não pode ser vazio");
            Nome = nome;

            if (string.IsNullOrEmpty(codigo))
                throw new ArgumentException("O código do banco não pode ser vazio");
            Codigo = codigo;

            PercentualDeJuros = percentualDeJuros;
        }
    }
}
