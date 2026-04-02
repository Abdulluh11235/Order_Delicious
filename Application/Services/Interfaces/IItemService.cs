using Application.Commands;
using Application.DTOs;
using Domain;

namespace Application.Services.Interfaces;

public interface IItemService
{
     Task<Result<uint>> Create(CreateItem createItem,CancellationToken cancellationToken=default);
      Task<Result<ItemPageDto>> GetItemPaged(int pageNumber, int pageSize,CancellationToken cancellationToken=default);
    
       Task<Result<ItemDto?>> GetById(int id,
          CancellationToken cancellationToken = default);
    
      Task<Result<int>> Update(int id,UpdateItem updateItem,CancellationToken cancellationToken = default);
      public Task<Result<int>> RemoveRange(IEnumerable<int> ids,
          CancellationToken cancellationToken = default);
      Task<Result<int>> RemoveById(int id,CancellationToken cancellationToken=default);
}