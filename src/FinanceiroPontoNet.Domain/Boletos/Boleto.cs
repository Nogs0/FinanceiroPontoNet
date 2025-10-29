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

        private Boleto() { }

        public Boleto(
            string nomeDoPagador,
            string documentoDoPagador,
            string nomeDoBeneficiario,
            decimal valor,
            DateTime dataDeVencimento,
            Guid bancoId
        )
        {
            Id = Guid.NewGuid();
            NomeDoPagador = nomeDoPagador;
            DocumentoDoPagador = documentoDoPagador;
            NomeDoBeneficiario = nomeDoBeneficiario;
            Valor = valor;
            DataDeVencimento = dataDeVencimento;
            BancoId = bancoId;
        }

        public void Atualizar(
            string nomeDoPagador,
            string nomeDoBeneficiario,
            string documentoDoPagador,
            DateTime dataDeVencimento,
            Guid bancoId,
            decimal valor
        )
        {
            if (string.IsNullOrEmpty(nomeDoPagador))
                throw new ArgumentException("O nome do pagador é obrigatório");
            NomeDoPagador = nomeDoPagador;

            if (string.IsNullOrEmpty(documentoDoPagador))
                throw new ArgumentException("O documento do pagador é obrigatório");
            DocumentoDoPagador = documentoDoPagador;

            if (string.IsNullOrEmpty(nomeDoBeneficiario))
                throw new ArgumentException("O nome do beneficiário é obrigatório");
            NomeDoBeneficiario = nomeDoBeneficiario;

            DataDeVencimento = dataDeVencimento;
            BancoId = bancoId;

            if (valor < 0)
                throw new ArgumentException("O valor de um boleto não pode ser negativo");

            Valor = valor;
        }
    }
}
