using DevLearning.AuthorAPI.Database;
using DevLearning.AuthorAPI.Repositories;
using DevLearning.AuthorAPI.Repositories.Interfaces;
using DevLearning.AuthorAPI.Services;
using DevLearning.AuthorAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<ConnectionDB>();

builder.Services.AddSingleton<IAuthorService, AuthorService>();
builder.Services.AddSingleton<IAuthorRepository, AuthorRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
