using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces.Repository;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Persistence;
public class ItemRepository:Repository<Item>,IItemRepository
{
    
    public ItemRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }
    public override async Task<IEnumerable<Item>> GetEntityPaged(int pageNumber, int pageSize)
    { 
        int offset = (pageNumber - 1) * pageSize;
        var queryWithDependants = DbSetIncludeImageAndCategory();
        return await queryWithDependants.Skip(offset).Take(pageSize).ToListAsync();
    }
    public override async Task<Item?> GetFirstOrDefault(Expression<Func<Item, bool>> condition)
    {
        var queryWithDependants = DbSetIncludeImageAndCategory();
        return await queryWithDependants.FirstOrDefaultAsync(condition);
    }
    public override async Task<IEnumerable<Item>> FindEntityPaged(Expression<Func<Item, bool>> predicate, int pageNumber, int pageSize)
    {
        var queryWithDependants = DbSetIncludeImageAndCategory();
        return await queryWithDependants.Where(predicate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    
    private IQueryable<Item> DbSetIncludeImageAndCategory()
    {
        return DbSet.Include(it => it.Images).Include(it => it.Categories);
    }
}