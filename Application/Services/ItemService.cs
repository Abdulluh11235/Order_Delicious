using Application.Commands;
using Application.DTOs;
using Application.Services.Interfaces;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repository;

namespace Application.Services;

public class ItemService:IItemService
{
    private readonly IUnitOfWork _unitOfWork;
     private readonly IMapper _mapper;
    public ItemService(IMapper mapper,IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<uint>> Create(CreateItem createItem, CancellationToken cancellationToken = default)
    {
        var categoriesEnum = await  _unitOfWork.Categories.
            FindEntities(c => createItem.CategoryIds.Contains(c.Id),cancellationToken);
        var categories = categoriesEnum.ToList();
      
        if (categories.Count == 0) return new Result<uint>(false,"No category provided");
        var item =   _mapper.Map<Item>(createItem);
        item.Categories = categories;
          _unitOfWork.Items.Add(item);
          await _unitOfWork.SaveChanges(cancellationToken);
       
          var res=new Result<uint>(true){Value = (uint)item.Id} ;
          return res;
    }

   public async Task<Result<ItemPageDto>> GetItemPaged(int pageNumber, int pageSize,
        CancellationToken cancellationToken = default)
    {
        if(pageNumber < 1) return new Result<ItemPageDto>(false,"PageNumber must be greater than or equal to 1");
        if(pageSize < 1) return new Result<ItemPageDto>(false,"PageSize must be greater than or equal to 1");
        
        var items= await _unitOfWork.Items.GetEntityPaged(pageNumber, pageSize,cancellationToken);
        var count= await _unitOfWork.Items.Count();
        var itemsDto = _mapper.Map<IEnumerable<ItemDto>>(items);
        var itemPageDto = new ItemPageDto() {Items =  itemsDto, TotalSize = count}; 
        
        return new Result<ItemPageDto>() { Value = itemPageDto };
    }

    public async Task<Result<ItemDto?>> GetById(int id, CancellationToken cancellationToken = default)
    {
        if (id < 0) return new Result<ItemDto?>(false,"Id must be greater than 0");
        var item = await _unitOfWork.Items.GetFirstOrDefault(i => i.Id == id);
        if (item is null) return new Result<ItemDto?>(false,Result<Item?>.NotFoundError);
        var value = _mapper.Map<ItemDto>(item);
        return new Result<ItemDto?>(){Value  = value};
    }

    public async Task<Result<int>> Update(int id, UpdateItem updateItem,
        CancellationToken cancellationToken = default)
    {
        if(id < 0) return new Result<int>(false,"Id must be greater than 0");
        var item = await _unitOfWork.Items.GetFirstOrDefault(i => i.Id == id);
        if (item is null) return new Result<int>(false,Result<Item>.NotFoundError);
        var categoriesEnum = await  _unitOfWork.Categories.
            FindEntities(c => updateItem.CategoryIds.Contains(c.Id), cancellationToken);
         if(item.Categories.Count == 0) return new Result<int>(false,"No category provided");
         CopyValues(item, updateItem);
         _unitOfWork.Items.Update(item);
         await _unitOfWork.SaveChanges(cancellationToken);
         return new Result<int>();
    }

    private void CopyValues(Item item,UpdateItem updateItem)
    {
        item.Name = updateItem.Name;
        item.Description = updateItem.Description;
        item.Price = updateItem.Price;
        item.DiscountRate = updateItem.DiscountRate;
        item.IsAvailable = updateItem.IsAvailable;
        item.Images = _mapper.Map<ICollection<Image>>(updateItem.Images);
    }

    public async Task<Result<int>> RemoveRange(IEnumerable<int> ids,
        CancellationToken cancellationToken = default)
    {
        if (ids.Any(id => id <= 0)) return new Result<int>(false, "Id Must Be Greater Than Zero");
        await _unitOfWork.Categories.RemoveRange(ids);
        await _unitOfWork.SaveChanges(cancellationToken);
        return new Result<int>() ;
    }
   public async Task<Result<int>> RemoveById(int id, CancellationToken cancellationToken = default)
   {
       var item = await _unitOfWork.Items.GetFirstOrDefault(it => it.Id == id);
       if (item == null) return new Result<int>(false,Result<int>.NotFoundError);
       _unitOfWork.Items.Remove(item);
       await _unitOfWork.SaveChanges(cancellationToken);
       return new Result<int>();
   }
}