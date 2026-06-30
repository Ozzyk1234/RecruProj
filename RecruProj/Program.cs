using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RecruProj.Data;
using RecruProj.Dtos.ProjectDtos;
using RecruProj.Models.ProjectsName;
using RecruProj.Models.TaskItemName;
using RecruProj.Repository.TaskItemRepository;
using RecruProj.Respository.ProjectRepository;
using RecruProj.Respository.TaskItemRepository;
using RecruProj.Validators.GlobalExceptionHandler;
using RecruProj.Validators.ProjectValidator;
using RecruProj.Validators.TaskItemValidator;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
builder.Services.AddScoped<IValidator<CreateProjectsDTO>, ProjectValidator>();
builder.Services.AddScoped<IValidator<TaskItem>, TaskItemValidator>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();