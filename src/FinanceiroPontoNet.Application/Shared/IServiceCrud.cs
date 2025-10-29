namespace FinanceiroPontoNet.Application.Shared
{
    public interface IServiceCrud<TEntityDto>
    {
        Task<TEntityDto> CreateAsync(TEntityDto dto);
        Task<TEntityDto> GetAsync(Guid id);
        Task<List<TEntityDto>> GetAllAsync();
        Task<TEntityDto> UpdateAsync(TEntityDto dto);
        Task DeleteAsync(Guid id);
    }
}
