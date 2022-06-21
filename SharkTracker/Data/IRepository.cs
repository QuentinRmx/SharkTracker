#nullable enable
namespace SharkTracker.Data
{
    internal interface IRepository<TModel> where TModel : class, new()
    {
        Task InitConnection();

        Task<ERepositoryResponse> Add(TModel row);

        Task<ERepositoryResponse> Update(TModel row);

        Task<ERepositoryResponse> Delete(TModel row);
    }
}
