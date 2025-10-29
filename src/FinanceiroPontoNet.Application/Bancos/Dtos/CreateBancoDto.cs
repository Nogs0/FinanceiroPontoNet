namespace FinanceiroPontoNet.Application.Bancos.Dtos
{
    public class CreateBancoDto
    {
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public decimal PercentualDeJuros { get; set; }
    }
}
