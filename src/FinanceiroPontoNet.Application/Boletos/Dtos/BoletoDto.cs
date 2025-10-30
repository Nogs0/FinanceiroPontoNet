using System.ComponentModel.DataAnnotations;
using FinanceiroPontoNet.Application.Shared;
using FinanceiroPontoNet.Domain.Boletos;

namespace FinanceiroPontoNet.Application.Boletos.Dtos
{
    public class BoletoDto : BaseEntityDto
    {
        [MaxLength(256, ErrorMessage = "O nome do pagador deve ter no máximo 256 caracteres.")]
        [Required(ErrorMessage = "O nome do pagador é obrigatório.")]
        public string NomeDoPagador { get; set; }

        [MaxLength(14, ErrorMessage = "O documento do pagador deve ter no máximo 18 caracteres.")]
        [Required(ErrorMessage = "O documento do pagador é obrigatório.")]
        public string DocumentoDoPagador { get; set; }

        [MaxLength(256, ErrorMessage = "O nome do beneficiário deve ter no máximo 256 caracteres.")]
        [Required(ErrorMessage = "O nome do beneficiário é obrigatório.")]
        public string NomeDoBeneficiario { get; set; }

        [MaxLength(
            14,
            ErrorMessage = "O documento do beneficiário deve ter no máximo 18 caracteres."
        )]
        [Required(ErrorMessage = "O documento do beneficiário é obrigatório.")]
        public string DocumentoDoBeneficiario { get; set; }
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
