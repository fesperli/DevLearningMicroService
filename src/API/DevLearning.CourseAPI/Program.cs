using DevLearning.CourseAPI.Database;
using DevLearning.CourseAPI.Repositories;
using DevLearning.CourseAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<DbConnectionFactory>();

builder.Services.AddSingleton<CourseRepository>();
builder.Services.AddSingleton<CourseService>();

builder.Services.AddHttpClient("Author", client =>
    client.BaseAddress = new Uri("https://localhost:6001/author/"));

builder.Services.AddHttpClient("Category", client =>
    client.BaseAddress = new Uri("https://localhost:5001/category/"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
