using FluentAssertions;
using Moq;
using Tasky.Application.DTOs;
using Tasky.Application.Interfaces;
using Tasky.Application.Services;
using Tasky.Domain.Entities;
using TaskStatusEnum = Tasky.Domain.Enums.TaskStatus;

namespace Tasky.Application.Tests.Services;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _repositoryMock;
    private readonly TaskService _taskService;
    private readonly Guid _testId = Guid.NewGuid();
    private readonly Guid _nonExistentId = Guid.NewGuid();

    public TaskServiceTests()
    {
        _repositoryMock = new Mock<ITaskRepository>();
        _taskService = new TaskService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnAllTasks()
    {
        // Arrange
        List<TaskItem> tasks =
        [
            new() { Id = Guid.NewGuid(), Title = "Task 1", Status = TaskStatusEnum.Pending },
            new() { Id = Guid.NewGuid(), Title = "Task 2", Status = TaskStatusEnum.InProgress }
        ];
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

        // Act
        IEnumerable<TaskDto> result = await _taskService.GetAllTasksAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.Title == "Task 1");
        result.Should().Contain(t => t.Title == "Task 2");
    }

    [Fact]
    public async Task GetTaskByIdAsync_WhenTaskExists_ShouldReturnTask()
    {
        // Arrange
        TaskItem task = new() { Id = _testId, Title = "Test Task", Status = TaskStatusEnum.Pending };
        _repositoryMock.Setup(r => r.GetByIdAsync(_testId)).ReturnsAsync(task);

        // Act
        TaskDto? result = await _taskService.GetTaskByIdAsync(_testId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(_testId);
        result.Title.Should().Be("Test Task");
        result.Status.Should().Be(TaskStatusEnum.Pending);
    }

    [Fact]
    public async Task GetTaskByIdAsync_WhenTaskDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(_nonExistentId)).ReturnsAsync((TaskItem?)null);

        // Act
        TaskDto? result = await _taskService.GetTaskByIdAsync(_nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldCreateAndReturnTask()
    {
        // Arrange
        CreateTaskDto createDto = new()
        {
            Title = "New Task",
            Description = "New Description"
        };

        TaskItem createdTask = new()
        {
            Id = _testId,
            Title = createDto.Title,
            Description = createDto.Description,
            Status = TaskStatusEnum.Pending
        };

        _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<TaskItem>()))
            .ReturnsAsync(createdTask);

        // Act
        TaskDto result = await _taskService.CreateTaskAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("New Task");
        result.Description.Should().Be("New Description");
        result.Status.Should().Be(TaskStatusEnum.Pending);
        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_WhenTaskExists_ShouldUpdateAndReturnTask()
    {
        // Arrange
        TaskItem existingTask = new()
        {
            Id = _testId,
            Title = "Old Title",
            Description = "Old Description",
            Status = TaskStatusEnum.Pending
        };

        UpdateTaskDto updateDto = new()
        {
            Title = "Updated Title",
            Description = "Updated Description"
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(_testId)).ReturnsAsync(existingTask);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>()))
            .ReturnsAsync((TaskItem t) => t);

        // Act
        TaskDto? result = await _taskService.UpdateTaskAsync(_testId, updateDto);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Updated Title");
        result.Description.Should().Be("Updated Description");
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_WhenTaskDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        UpdateTaskDto updateDto = new() { Title = "Updated Title" };
        _repositoryMock.Setup(r => r.GetByIdAsync(_nonExistentId)).ReturnsAsync((TaskItem?)null);

        // Act
        TaskDto? result = await _taskService.UpdateTaskAsync(_nonExistentId, updateDto);

        // Assert
        result.Should().BeNull();
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public async Task ChangeTaskStatusAsync_WhenTaskExists_ShouldUpdateStatus()
    {
        // Arrange
        TaskItem existingTask = new()
        {
            Id = _testId,
            Title = "Test Task",
            Status = TaskStatusEnum.Pending
        };

        ChangeStatusDto statusDto = new() { Status = TaskStatusEnum.Completed };

        _repositoryMock.Setup(r => r.GetByIdAsync(_testId)).ReturnsAsync(existingTask);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>()))
            .ReturnsAsync((TaskItem t) => t);

        // Act
        TaskDto? result = await _taskService.ChangeTaskStatusAsync(_testId, statusDto);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be(TaskStatusEnum.Completed);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public async Task ChangeTaskStatusAsync_WhenTaskDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        ChangeStatusDto statusDto = new() { Status = TaskStatusEnum.Completed };
        _repositoryMock.Setup(r => r.GetByIdAsync(_nonExistentId)).ReturnsAsync((TaskItem?)null);

        // Act
        TaskDto? result = await _taskService.ChangeTaskStatusAsync(_nonExistentId, statusDto);

        // Assert
        result.Should().BeNull();
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public async Task DeleteTaskAsync_WhenTaskExists_ShouldReturnTrue()
    {
        // Arrange
        _repositoryMock.Setup(r => r.DeleteAsync(_testId)).ReturnsAsync(true);

        // Act
        bool result = await _taskService.DeleteTaskAsync(_testId);

        // Assert
        result.Should().BeTrue();
        _repositoryMock.Verify(r => r.DeleteAsync(_testId), Times.Once);
    }

    [Fact]
    public async Task DeleteTaskAsync_WhenTaskDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        _repositoryMock.Setup(r => r.DeleteAsync(_nonExistentId)).ReturnsAsync(false);

        // Act
        bool result = await _taskService.DeleteTaskAsync(_nonExistentId);

        // Assert
        result.Should().BeFalse();
    }
}
