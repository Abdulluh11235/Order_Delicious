using Domain.Interfaces.Repository;

namespace Domain.Interfaces;

public interface IUnitOfWork
{
   ICategoryRepository Categories { get; set; }
   IItemRepository Items { get; set; }
   
   Task SaveChanges(CancellationToken cancellationToken);
}