namespace FinanceiroPontoNet.Domain.Shared.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() { }

        public NotFoundException(string message)
            : base(message) { }

        public NotFoundException(string entityName, object id)
            : base($"A entidade '{entityName}' com a chave: '{id}' não foi encontrada.") { }
    }
}
