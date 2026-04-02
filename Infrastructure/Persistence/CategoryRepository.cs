using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces.Repository;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Persistence;

public class CategoryRepository:Repository<Category>,ICategoryRepository
{
    public CategoryRepository(AppDbContext appDbContext):base(appDbContext)
    {
        
    }
    public override async Task<IEnumerable<Category>> GetEntityPaged(int pageNumber, int pageSize,CancellationToken cancellationToken)
    { 
        int offset = (pageNumber - 1) * pageSize;
        var dbSetImageIncluded = DbSetIncludeImage();
        return await dbSetImageIncluded.Skip(offset).Take(pageSize).ToListAsync(cancellationToken);
    }
    public override async Task<Category?> GetFirstOrDefault(Expression<Func<Category, bool>> condition)
    {
        var dbSetImageIncluded = DbSetIncludeImage();
        return await dbSetImageIncluded.FirstOrDefaultAsync(condition);
    }
    public override async Task<IEnumerable<Category>> FindEntities(Expression<Func<Category, bool>> predicate,CancellationToken cancellationToken)
    {
        var dbSetImageIncluded = DbSetIncludeImage();
        return await dbSetImageIncluded.Where(predicate).ToListAsync(cancellationToken);
    }

    
    private IQueryable<Category> DbSetIncludeImage()
    {
       return DbSet.Include(cat=>cat.Image);
    }

}

