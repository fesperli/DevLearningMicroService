using DevLearning.StudentAPI.Data;
using DevLearning.StudentAPI.Repositories;
using DevLearning.StudentAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MongoDBConnection>();

builder.Services.AddSingleton<StudentRepository>();
builder.Services.AddSingleton<StudentService>();

builder.Services.AddHttpClient("courseClient", client => client.BaseAddress = new Uri("https://localhost:7001/api/Course/"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
