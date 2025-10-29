using System.ComponentModel.DataAnnotations;

namespace FinanceiroPontoNet.Application.Bancos.Dtos
{
    public class CreateBancoDto
    {
        [MaxLength(256, ErrorMessage = "O nome deve ter no máximo 256 caracteres.")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; } = "";

        [MaxLength(64, ErrorMessage = "O código deve ter no máximo 64 caracteres.")]
        [Required(ErrorMessage = "O código é obrigatório.")]
        public string Codigo { get; set; } = "";
        public decimal PercentualDeJuros { get; set; }
    }
}
