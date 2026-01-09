using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tasky.Domain.Entities;
using TaskStatusEnum = Tasky.Domain.Enums.TaskStatus;

namespace Tasky.Infrastructure.Persistence.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        // Mapear a la tabla "Tasks"
        builder.ToTable("Tasks");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        // Convertir Enum a String para mejor legibilidad en DB
        builder.Property(t => t.Status)
            .HasConversion(
                v => v.ToString(),
                v => (TaskStatusEnum)Enum.Parse(typeof(TaskStatusEnum), v));

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .IsRequired();
    }
}
