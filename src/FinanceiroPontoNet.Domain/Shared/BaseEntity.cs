using System.ComponentModel.DataAnnotations;

namespace FinanceiroPontonet.Domain.Shared
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
