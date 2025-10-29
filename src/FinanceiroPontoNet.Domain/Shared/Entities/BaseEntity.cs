using System.ComponentModel.DataAnnotations;

namespace FinanceiroPontoNet.Domain.Shared
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
