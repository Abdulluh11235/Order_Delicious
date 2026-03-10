using Application.Commands;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repository;

namespace Application.Services;

public class CategoryServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper; 
    public CategoryServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }  
   public async Task<uint> Add(CreateCategory createCategory,CancellationToken cancellationToken=default)
   {
      var category = _mapper.Map<Category>(createCategory);
      
      _unitOfWork.Categories.Add(category);
      await _unitOfWork.SaveChanges(cancellationToken);
      return (uint) category.Id;
   }

   // public async Task<Result<CategoryPageDto> GetEnittyPaged(int pageNumber, int pageSize)
   // {
   //     if (pageNumber < 1) return 
   //         new Result<IEnumerable<Category>>(false,"Page Number Must Be Greater Than Zero");
   //     if (pageSize < 1) return 
   //         new Result<IEnumerable<Category>>(false,"Page Size Must Be Greater Than Zero"); 
   //     
   //     var page = await  _unitOfWork.Categories.GetEntityPaged(pageNumber, pageSize);
   //      if(!page.Any()) return new Result<IEnumerable<Category>>(false,"No Category Found");
   //     return new Result<IEnumerable<Category>>(true) {Value = page};
   // }
   
}