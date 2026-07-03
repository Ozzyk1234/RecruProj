using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RecruProj.Data;
using RecruProj.Dtos.ProjectDtos;
using RecruProj.Respository.ProjectRepository;
using RecruProj.Validators.CustomExceptions;

namespace RecrtuProj.Tests;

public class ProjectRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly ProjectRepository _repository;

    public ProjectRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new ProjectRepository(_context);
    }

    [Fact]
    public async Task CreateProject_ShouldReturnCreatedProject()
    {
        //Arrange
        var project = new CreateProjectsDTO(
            "Test Project",
            "Test Description");

        //Act

        var result = await _repository.CreateProject(project);


        //Assert
        Assert.NotNull(result);
        Assert.Equal("Test Project", result.name);
        Assert.Equal("Test Description", result.description);
        Assert.Equal(DateTime.UtcNow.Date, result.createdAt.Date);
    }
    [Fact]
    public async Task CreateProject_ShouldThrowConflictException_WhenProjectNameAlreadyExists()
    {
        //Arrange
        var project = new CreateProjectsDTO(
            "Test Project",
            "Test Description");
        var project2 = new CreateProjectsDTO(
            "Test Project",
            "Test Description");

        //Act

        await _repository.CreateProject(project);


        //Assert
        await Assert.ThrowsAsync<ConflictException>(async () => await _repository.CreateProject(project2));

    }
}