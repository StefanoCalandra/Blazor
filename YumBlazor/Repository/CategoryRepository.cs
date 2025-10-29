using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YumBlazor.Data;
using YumBlazor.Repository.IRepository;

namespace YumBlazor.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Category> CreateAsync(Category obj, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(obj);

            await _db.Category.AddAsync(obj, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return obj;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var obj = await _db.Category.FindAsync(new object?[] { id }, cancellationToken);
            if (obj is null)
            {
                return false;
            }
            _db.Category.Remove(obj);
            return await _db.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<Category?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _db.Category.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Category.AsNoTracking().OrderBy(category => category.Name).ToListAsync(cancellationToken);
        }

        public async Task<Category?> UpdateAsync(Category obj, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(obj);

            var objFromDb = await _db.Category.FirstOrDefaultAsync(u => u.Id == obj.Id, cancellationToken);
            if (objFromDb is null)
            {
                return null;
            }
            objFromDb.Name = obj.Name;
            await _db.SaveChangesAsync(cancellationToken);
            return objFromDb;
        }
    }
}
