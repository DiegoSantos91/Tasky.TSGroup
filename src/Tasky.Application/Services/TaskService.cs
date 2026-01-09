using Tasky.Application.DTOs;
using Tasky.Application.Interfaces;
using Tasky.Domain.Entities;
using TaskStatusEnum = Tasky.Domain.Enums.TaskStatus;

namespace Tasky.Application.Services;

public class TaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
    {
        IEnumerable<TaskItem> tasks = await _repository.GetAllAsync();
        return tasks.Select(MapToDto);
    }

    public async Task<TaskDto?> GetTaskByIdAsync(Guid id)
    {
        TaskItem? task = await _repository.GetByIdAsync(id);
        return task == null ? null : MapToDto(task);
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createDto)
    {
        TaskItem task = new TaskItem
        {
            Title = createDto.Title,
            Description = createDto.Description,
            Status = TaskStatusEnum.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        TaskItem createdTask = await _repository.CreateAsync(task);
        return MapToDto(createdTask);
    }

    public async Task<TaskDto?> UpdateTaskAsync(Guid id, UpdateTaskDto updateDto)
    {
        TaskItem? task = await _repository.GetByIdAsync(id);
        if (task == null) return null;

        task.Title = updateDto.Title;
        task.Description = updateDto.Description;
        task.UpdatedAt = DateTime.UtcNow;

        TaskItem updatedTask = await _repository.UpdateAsync(task);
        return MapToDto(updatedTask);
    }

    public async Task<TaskDto?> ChangeTaskStatusAsync(Guid id, ChangeStatusDto statusDto)
    {
        TaskItem? task = await _repository.GetByIdAsync(id);
        if (task == null) return null;

        task.Status = statusDto.Status;
        task.UpdatedAt = DateTime.UtcNow;

        TaskItem updatedTask = await _repository.UpdateAsync(task);
        return MapToDto(updatedTask);
    }

    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static TaskDto MapToDto(TaskItem task)
    {
        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
