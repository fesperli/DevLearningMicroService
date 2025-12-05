using API.Database;
using API.Repositories;
using API.Repositories.Interfaces;
using API.Services;
using API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<DbConnectionFactory>();

builder.Services.AddSingleton<CourseRepository>();
builder.Services.AddSingleton<CourseService>();

builder.Services.AddSingleton<ICategoryService, CategoryService>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();

builder.Services.AddSingleton<IAuthorService, AuthorService>();
builder.Services.AddSingleton<IAuthorRepository, AuthorRepository>();

builder.Services.AddSingleton<StudentRepository>();
builder.Services.AddSingleton<StudentService>();

builder.Services.AddSingleton<CareerRepository>();
builder.Services.AddSingleton<CareerService>();



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
// devTeste1////////
