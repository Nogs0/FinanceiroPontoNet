namespace FinanceiroPontoNet.Application.Shared
{
    public interface IServiceCrud<TEntityDto, TCreateEntityDto>
    {
        Task<TEntityDto> CreateAsync(TCreateEntityDto dto);
        Task<TEntityDto> GetAsync(Guid id);
        Task<List<TEntityDto>> GetAllAsync();
        Task UpdateAsync(TEntityDto dto);
        Task DeleteAsync(Guid id);
    }
}
