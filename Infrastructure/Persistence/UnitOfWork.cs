using Domain;
using Domain.Interfaces;
using Domain.Interfaces.Repository;
using Infrastructure.Persistence.Data;

namespace Infrastructure.Persistence;

public class UnitOfWork:IUnitOfWork
{
  readonly AppDbContext _db;
  public ICategoryRepository Categories { get; set; }
  public IItemRepository Items { get; set; }
  public IImageRepository Images { get; set; }
  public UnitOfWork(AppDbContext db, ICategoryRepository categories, IItemRepository items
  ,IImageRepository images)
  {
   _db = db; 
   Categories = categories;
   Items = items;
   Images = images;
  }
 

  
  public async Task SaveChanges(CancellationToken cancellationToken=default)
  {
      await _db.SaveChangesAsync(cancellationToken);
  }
}