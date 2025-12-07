using DevLearning.StudentAPI.Data;
using DevLearning.StudentAPI.Repositories;
using DevLearning.StudentAPI.Repositories.Interface;
using DevLearning.StudentAPI.Services;
using DevLearning.StudentAPI.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MongoDBConnection>();

builder.Services.AddSingleton<StudentRepository>();
builder.Services.AddSingleton<StudentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
