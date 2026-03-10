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
    public override async Task<IEnumerable<Category>> GetEntityPaged(int pageNumber, int pageSize)
    { 
        int offset = (pageNumber - 1) * pageSize;
        var dbSetImageIncluded = DbSetIncludeImage();
        return await dbSetImageIncluded.Skip(offset).Take(pageSize).ToListAsync();
    }
    public override async Task<Category?> GetFirstOrDefault(Expression<Func<Category, bool>> condition)
    {
        var dbSetImageIncluded = DbSetIncludeImage();
        return await dbSetImageIncluded.FirstOrDefaultAsync(condition);
    }
    public override async Task<IEnumerable<Category>> FindEntityPaged(Expression<Func<Category, bool>> predicate, int pageNumber, int pageSize)
    {
        var dbSetImageIncluded = DbSetIncludeImage();
        return await dbSetImageIncluded.Where(predicate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    
    private IQueryable<Category> DbSetIncludeImage()
    {
       return DbSet.Include(cat=>cat.Image);
    }

}

