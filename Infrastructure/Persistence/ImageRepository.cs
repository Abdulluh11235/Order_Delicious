using Domain.Entities;
using Domain.Interfaces.Repository;
using Infrastructure.Persistence.Data;

namespace Infrastructure.Persistence;

public class ImageRepository:Repository<Image>,IImageRepository
{
    public ImageRepository(AppDbContext db):base(db)
    {
        
    }
}