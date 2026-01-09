using TaskStatusEnum = Tasky.Domain.Enums.TaskStatus;

namespace Tasky.Application.DTOs;

public class ChangeStatusDto
{
    public TaskStatusEnum Status { get; set; }
}
