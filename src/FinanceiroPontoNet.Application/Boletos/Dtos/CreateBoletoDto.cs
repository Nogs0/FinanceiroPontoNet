namespace FinanceiroPontoNet.Application.Boletos.Dtos
{
    public class CreateBoletoDto
    {
        public string NomeDoPagador { get; set; } = "";
        public string DocumentoDoPagador { get; set; } = "";
        public string NomeDoBeneficiario { get; set; } = "";
        public decimal Valor { get; set; }
        public DateTime DataDeVencimento { get; set; }
        public Guid BancoId { get; set; }
    }
}
