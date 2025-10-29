using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YumBlazor.Data;

namespace YumBlazor.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category obj, CancellationToken cancellationToken = default);
        Task<Category?> UpdateAsync(Category obj, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<Category?> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
