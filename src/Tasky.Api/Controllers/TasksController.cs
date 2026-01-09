using Microsoft.AspNetCore.Mvc;
using Tasky.Application.DTOs;
using Tasky.Application.Services;
using FluentValidation;

namespace Tasky.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskService _taskService;
    private readonly IValidator<CreateTaskDto> _createValidator;
    private readonly IValidator<UpdateTaskDto> _updateValidator;
    private readonly IValidator<ChangeStatusDto> _statusValidator;
    private readonly ILogger<TasksController> _logger;

    public TasksController(
        TaskService taskService,
        IValidator<CreateTaskDto> createValidator,
        IValidator<UpdateTaskDto> updateValidator,
        IValidator<ChangeStatusDto> statusValidator,
        ILogger<TasksController> logger)
    {
        _taskService = taskService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _statusValidator = statusValidator;
        _logger = logger;
    }

    /// <summary>
    /// Get all tasks
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll()
    {
        _logger.LogInformation("Getting all tasks");
        IEnumerable<TaskDto> tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    /// <summary>
    /// Get a task by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> GetById(Guid id)
    {
        _logger.LogInformation("Getting task with ID: {TaskId}", id);
        TaskDto? task = await _taskService.GetTaskByIdAsync(id);

        if (task == null)
        {
            _logger.LogWarning("Task with ID {TaskId} not found", id);
            return NotFound(new { message = $"Task with ID {id} not found" });
        }

        return Ok(task);
    }

    /// <summary>
    /// Create a new task
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskDto createDto)
    {
        FluentValidation.Results.ValidationResult validationResult = await _createValidator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        _logger.LogInformation("Creating new task with title: {Title}", createDto.Title);
        TaskDto task = await _taskService.CreateTaskAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    /// <summary>
    /// Update an existing task
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> Update(Guid id, [FromBody] UpdateTaskDto updateDto)
    {
        FluentValidation.Results.ValidationResult validationResult = await _updateValidator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        _logger.LogInformation("Updating task with ID: {TaskId}", id);
        TaskDto? task = await _taskService.UpdateTaskAsync(id, updateDto);

        if (task == null)
        {
            _logger.LogWarning("Task with ID {TaskId} not found for update", id);
            return NotFound(new { message = $"Task with ID {id} not found" });
        }

        return Ok(task);
    }

    /// <summary>
    /// Change task status
    /// </summary>
    [HttpPatch("{id}/status")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> ChangeStatus(Guid id, [FromBody] ChangeStatusDto statusDto)
    {
        FluentValidation.Results.ValidationResult validationResult = await _statusValidator.ValidateAsync(statusDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        _logger.LogInformation("Changing status for task with ID: {TaskId} to {Status}", id, statusDto.Status);
        TaskDto? task = await _taskService.ChangeTaskStatusAsync(id, statusDto);

        if (task == null)
        {
            _logger.LogWarning("Task with ID {TaskId} not found for status change", id);
            return NotFound(new { message = $"Task with ID {id} not found" });
        }

        return Ok(task);
    }

    /// <summary>
    /// Delete a task
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation("Deleting task with ID: {TaskId}", id);
        bool result = await _taskService.DeleteTaskAsync(id);

        if (!result)
        {
            _logger.LogWarning("Task with ID {TaskId} not found for deletion", id);
            return NotFound(new { message = $"Task with ID {id} not found" });
        }

        return NoContent();
    }
}
