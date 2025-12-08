using DevLearning.CareerAPI.Data;
using DevLearning.CareerAPI.Repositories.Interfaces;
using DevLearning.CareerAPI.Service.Interfaces;
using DevLearning.Repositories;
using DevLearning.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration["MongoDBSettings:ConnectionURI"]));

builder.Services.AddScoped<ICareerRepository, CareerRepository>();

builder.Services.AddHttpClient<ICareerService, CareerService>(client =>
    client.BaseAddress = new Uri("https://localhost:7001/api/course/"));

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
