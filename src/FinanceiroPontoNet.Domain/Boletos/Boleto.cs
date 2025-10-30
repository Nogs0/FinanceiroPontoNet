using System.ComponentModel.DataAnnotations;
using FinanceiroPontoNet.Domain.Bancos;
using FinanceiroPontoNet.Domain.Shared;
using FinanceiroPontoNet.Domain.Shared.Utils;

namespace FinanceiroPontoNet.Domain.Boletos
{
    public class Boleto : FullAuditedEntity, ISoftDelete
    {
        [MaxLength(256)]
        [Required]
        public string NomeDoPagador { get; set; }

        [MaxLength(14)]
        [Required]
        public string DocumentoDoPagador { get; set; }

        [MaxLength(256)]
        [Required]
        public string NomeDoBeneficiario { get; set; }

        [MaxLength(14)]
        [Required]
        public string DocumentoDoBeneficiario { get; set; }

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
            string documentoDoBeneficiario,
            decimal valor,
            DateTime dataDeVencimento,
            Guid bancoId
        )
        {
            Id = Guid.NewGuid();
            if (string.IsNullOrEmpty(nomeDoPagador))
                throw new ArgumentException("O nome do pagador é obrigatório");
            NomeDoPagador = nomeDoPagador;

            documentoDoPagador = CpfCpnjUtils.RemoveFormat(documentoDoPagador);
            if (!CpfCpnjUtils.Validate(documentoDoPagador))
                throw new ArgumentException("O documento do pagador é inválido.");
            DocumentoDoPagador = documentoDoPagador;

            if (string.IsNullOrEmpty(nomeDoBeneficiario))
                throw new ArgumentException("O nome do beneficiário é obrigatório.");
            NomeDoBeneficiario = nomeDoBeneficiario;

            documentoDoBeneficiario = CpfCpnjUtils.RemoveFormat(documentoDoBeneficiario);
            if (!CpfCpnjUtils.Validate(documentoDoBeneficiario))
                throw new ArgumentException("O documento do beneficiário é inválido.");
            DocumentoDoBeneficiario = documentoDoBeneficiario;

            if (valor < 0)
                throw new ArgumentException("O valor de um boleto não pode ser negativo");

            Valor = valor;
            DataDeVencimento = dataDeVencimento;
            BancoId = bancoId;
        }

        public void Atualizar(
            string nomeDoPagador,
            string documentoDoPagador,
            string nomeDoBeneficiario,
            string documentoDoBeneficiario,
            decimal valor,
            DateTime dataDeVencimento,
            Guid bancoId
        )
        {
            if (string.IsNullOrEmpty(nomeDoPagador))
                throw new ArgumentException("O nome do pagador é obrigatório");
            NomeDoPagador = nomeDoPagador;

            documentoDoPagador = CpfCpnjUtils.RemoveFormat(documentoDoPagador);
            if (!CpfCpnjUtils.Validate(documentoDoPagador))
                throw new ArgumentException("O documento do pagador é inválido.");
            DocumentoDoPagador = documentoDoPagador;

            if (string.IsNullOrEmpty(nomeDoBeneficiario))
                throw new ArgumentException("O nome do beneficiário é obrigatório.");
            NomeDoBeneficiario = nomeDoBeneficiario;

            documentoDoBeneficiario = CpfCpnjUtils.RemoveFormat(documentoDoBeneficiario);
            if (!CpfCpnjUtils.Validate(documentoDoBeneficiario))
                throw new ArgumentException("O documento do beneficiário é inválido.");
            DocumentoDoBeneficiario = documentoDoBeneficiario;

            DataDeVencimento = dataDeVencimento;
            BancoId = bancoId;

            if (valor < 0)
                throw new ArgumentException("O valor de um boleto não pode ser negativo");

            Valor = valor;
        }
    }
}
