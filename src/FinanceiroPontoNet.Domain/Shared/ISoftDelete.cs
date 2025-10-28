namespace FinanceiroPontonet.Domain.Shared
{
    public interface ISoftDelete
    {
        public DateTime? DeletedAt { get; set; }
    }
}
