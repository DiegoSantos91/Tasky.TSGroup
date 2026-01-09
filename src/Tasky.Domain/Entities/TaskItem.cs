using TaskStatusEnum = Tasky.Domain.Enums.TaskStatus;

namespace Tasky.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    private string _title = string.Empty;
    public string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Title cannot be empty", nameof(Title));

            if (value.Length > 200)
                throw new ArgumentException("Title cannot exceed 200 characters", nameof(Title));

            _title = value;
        }
    }

    public string? Description { get; set; }

    public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
