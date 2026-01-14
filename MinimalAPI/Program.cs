using Microsoft.EntityFrameworkCore;
using MinimalAPI.Interface;
using MinimalAPI.Models;
using MinimalAPI.Repository;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("BiappsDBCon")));

//builder.Services.AddDbContext<DatabaseContext>
//    (options => options.UseInMemoryDatabase("InMemoryDbForTesting"));

builder.Services.AddTransient<IStudentManager, StudentManagerRepository>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Machine API

app.MapGet("api/Get_AllStudent", (IStudentManager service, string? sSearchText
    , int PageNumber, int PageSize, string? SortBy, string? SortOrder) =>
{
    return service.Get_AllStudent (sSearchText
                       , PageNumber
                       , PageSize
                       , SortBy
                       , SortOrder);
})
.WithName("Get_AllStudent");

app.MapGet("/api/Get_StudentById", (IStudentManager service, int Id) =>
{
    return service.Get_SudentById(Id);
})
.WithName("Get_StudentById");

app.MapPost("/api/Save_Student", (Student obj, IStudentManager service) =>
{
    PostResult _result = new PostResult();
    _result.Id = service.Save_Student(obj);
    _result.Message = "Student has been saved successfully";

    return Results.Ok(_result);
});

app.MapDelete("/api/Remove_Student/{Id}", (int Id, IStudentManager service) =>
{
    service.Remove_Student(Id);
    return Results.Ok("Student has been deleted successfully");
});

#endregion

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }
