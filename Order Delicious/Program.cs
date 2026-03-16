using Application.Mappings;
using Application.Services;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repository;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

#region  Infrastructure
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IItemRepository,ItemRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion
#region Application 
    
builder.Services.AddSingleton<IMapper>(provider =>
{
    var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

    var config = new MapperConfiguration(cfg =>
    {
        cfg.AddMaps(typeof(CategoryProfile).Assembly);
    }, loggerFactory);

    return config.CreateMapper();
});
    builder.Services.AddScoped<ICategoryService,CategoryService>();
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();