using System.Linq.Expressions;
using Application.Commands;
using Application.DTOs;
using Application.Services.Interfaces;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper; 
    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }  
   public async Task<Result<uint>> Create(CreateCategory createCategory,CancellationToken cancellationToken=default)
   {
      var category = _mapper.Map<Category>(createCategory);
      
      _unitOfWork.Categories.Add(category);
      await _unitOfWork.SaveChanges(cancellationToken);
      var res=new Result<uint>(true){Value = (uint)category.Id} ;
      return res;
   }

   public async Task<Result<CategoryPageDto>> GetCategoryPaged(int pageNumber, int pageSize,CancellationToken cancellationToken=default)
   {
       if (pageNumber < 1) return 
           new Result<CategoryPageDto>(false,"Page Number Must Be Greater Than Zero");
       if (pageSize < 1) return 
           new Result<CategoryPageDto>(false,"Page Size Must Be Greater Than Zero");
       
       var page = await  _unitOfWork.Categories.GetEntityPaged(pageNumber, pageSize);
           var categoriesDto = _mapper.Map<List<CategoryDto>>(page);
       
       var count = await _unitOfWork.Categories.Count();
       
       var resultDto = new CategoryPageDto() { TotalSize = count, Categories = categoriesDto };
           return new Result<CategoryPageDto>(true)
               { Value = resultDto }; 
   }

   public async Task<Result<CategoryDto?>> GetById(int id, CancellationToken cancellationToken = default)
   {
       if(id <= 0) return new Result<CategoryDto?>(false,"Id Must Be Greater Than Zero");
       var val= await  _unitOfWork.Categories.GetFirstOrDefault(cat=>cat.Id == id);
       return new Result<CategoryDto?>(true){ Value = _mapper.Map<CategoryDto>(val) };
   }
   

   public async Task<Result<IEnumerable<CategoryDto>>> FindCategoryPaged(Expression<Func<Category, bool>> predicate,
       int pageNumber,int pageSize,CancellationToken cancellationToken=default)
   {
        var categories = await _unitOfWork.Categories.FindEntityPaged(predicate,pageNumber, pageSize);
       return new Result<IEnumerable<CategoryDto>>(true) {Value =_mapper.Map<List<CategoryDto>>(categories) };
   }

   public async Task<Result<int>> Update(int id,UpdateCategory updateCategory, CancellationToken cancellationToken = default)
   {
       if(id <= 0) return new Result<int>(false, "Id Must Be Greater Than Zero");
       var category = await _unitOfWork.Categories.GetFirstOrDefault(c => c.Id == id);
       if (category == null) return new Result<int>(false,Result<int>.NotFoundError);
       category.Name = updateCategory.Name;
       UpdateCategoryImage(category.Image,updateCategory.Image);
       _unitOfWork.Categories.Update(category);
       await _unitOfWork.SaveChanges(cancellationToken);
       return new Result<int>() ;
   }
   private void UpdateCategoryImage(Image img,UpdateImage uimg)
   {
       img.Title = uimg.Title;
       img.AltText = uimg.AltText;
       img.Url = uimg.Url;
   }


   public async Task<Result<int>> RemoveRange(IEnumerable<int> ids,CancellationToken cancellationToken=default)
   {
       if (ids.Any(id => id <= 0)) return new Result<int>(false, "Id Must Be Greater Than Zero");
       await _unitOfWork.Categories.RemoveRange(ids);
       await _unitOfWork.SaveChanges(cancellationToken);
       return new Result<int>() ;
   }
   public async Task<Result<int>> RemoveById(int id,CancellationToken cancellationToken=default)
   {
       var category = await _unitOfWork.Categories.GetFirstOrDefault(c => c.Id == id);
       if (category == null) return new Result<int>(false,Result<int>.NotFoundError);
       _unitOfWork.Categories.Remove(category);
       await _unitOfWork.SaveChanges(cancellationToken);
       return new Result<int>();

   }
}