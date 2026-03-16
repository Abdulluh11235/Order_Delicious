using System.Linq.Expressions;
using Application.Commands;
using Application.DTOs;
using Domain;
using Domain.Entities;

namespace Application.Services.Interfaces;

public interface ICategoryService
{
    Task<Result<uint>> Create(CreateCategory createCategory,CancellationToken cancellationToken=default);
    Task<Result<CategoryPageDto>> GetCategoryPaged(int pageNumber, int pageSize,CancellationToken cancellationToken=default);

    Task<Result<CategoryDto?>> GetById(int id,
        CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<CategoryDto>>> FindCategoryPaged(Expression<Func<Category, bool>> predicate,
        int pageNumber,int pageSize,CancellationToken cancellationToken=default);
    Task<Result<int>> Update(int id,UpdateCategory updateCategory,CancellationToken cancellationToken = default);
    public Task<Result<int>> RemoveRange(IEnumerable<int> ids,
        CancellationToken cancellationToken = default);
    Task<Result<int>> RemoveById(int id,CancellationToken cancellationToken=default);
}