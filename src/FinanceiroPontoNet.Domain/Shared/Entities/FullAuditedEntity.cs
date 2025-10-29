namespace FinanceiroPontoNet.Domain.Shared
{
    public class FullAuditedEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
