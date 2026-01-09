using FluentValidation;
using Tasky.Application.DTOs;
using TaskStatusEnum = Tasky.Domain.Enums.TaskStatus;

namespace Tasky.Application.Validators;

public class ChangeStatusDtoValidator : AbstractValidator<ChangeStatusDto>
{
    public ChangeStatusDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid TaskStatus value");
    }
}
