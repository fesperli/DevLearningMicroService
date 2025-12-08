
using DevLearning.CategoryAPI.Database;
using DevLearning.CategoryAPI.Repositories;
using DevLearning.CategoryAPI.Repositories.Interfaces;
using DevLearning.CategoryAPI.Services;
using DevLearning.CategoryAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddSingleton<DbConnectionFactory>();

builder.Services.AddSingleton<ICategoryService, CategoryService>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();

var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
