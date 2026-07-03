using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RecruProj.Data;
using RecruProj.Dtos.ProjectDtos;
using RecruProj.Dtos.TaskItemDtos;
using RecruProj.Models.ProjectsName;
using RecruProj.Models.TaskItemName;
using RecruProj.Repository.TaskItemRepository;
using RecruProj.Respository.ProjectRepository;
using RecruProj.Respository.TaskItemRepository;
using RecruProj.Validators.GlobalExceptionHandler;
using RecruProj.Validators.PaginationValidator;
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
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
        maxRetryCount: 5,
        maxRetryDelay: TimeSpan.FromSeconds(10),
        errorNumbersToAdd: null)));
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
builder.Services.AddScoped<IValidator<CreateProjectsDTO>, ProjectValidator>();
builder.Services.AddScoped<IValidator<CreateUpdateTaskItemDTO>, TaskItemValidator>();
builder.Services.AddScoped<IValidator<PaginationValidatorDTO>, PaginationValidator>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var retries = 10;
    while (retries > 0)
    {
        try
        {
            await db.Database.MigrateAsync();
            break;
        }
        catch (Exception ex)
        {
            retries--;
            Console.WriteLine($"Migration failed, retries left: {retries}. Error: {ex.Message}");
            if (retries == 0) throw;
            await Task.Delay(3000);
        }
    }
}
app.UseSwagger();
    app.UseSwaggerUI();
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();