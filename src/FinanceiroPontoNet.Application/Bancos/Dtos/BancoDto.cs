using System.ComponentModel.DataAnnotations;
using FinanceiroPontoNet.Application.Shared;
using FinanceiroPontoNet.Domain.Bancos;
using FinanceiroPontoNet.Domain.Shared;

namespace FinanceiroPontoNet.Application.Bancos.Dtos
{
    public class BancoDto : BaseEntityDto
    {
        [MaxLength(256, ErrorMessage = "O nome deve ter no máximo 256 caracteres.")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [MaxLength(64, ErrorMessage = "O código deve ter no máximo 64 caracteres.")]
        [Required(ErrorMessage = "O código é obrigatório.")]
        public string Codigo { get; set; }

        public decimal PercentualDeJuros { get; set; }

        public BancoDto() { }

        public BancoDto(Banco entity)
        {
            Id = entity.Id;
            Nome = entity.Nome;
            Codigo = entity.Codigo;
            PercentualDeJuros = entity.PercentualDeJuros;
        }
    }
}
