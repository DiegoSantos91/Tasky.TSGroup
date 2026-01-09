using FluentValidation;
using Serilog;
using Tasky.Application.Services;
using Tasky.Application.Validators;
using Tasky.Infrastructure;
using Tasky.Infrastructure.Persistence;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/tasky-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting Tasky API");

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();

    // Add Infrastructure (DbContext, Repositories)
    builder.Services.AddInfrastructure(builder.Configuration);

    // Add Application Services
    builder.Services.AddScoped<TaskService>();

    // Add FluentValidation Validators
    builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskDtoValidator>();

    // Add Controllers
    builder.Services.AddControllers();

    // Add Health Checks
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<TaskyDbContext>("database");

    // Add Swagger/OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Add CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    // Add ProblemDetails
    builder.Services.AddProblemDetails();

    WebApplication app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tasky API v1");
            c.RoutePrefix = string.Empty; // Swagger at root
        });
    }

    app.UseHttpsRedirection();

    app.UseCors("AllowAll");

    app.UseAuthorization();

    app.MapControllers();

    // Map Health Checks
    app.MapHealthChecks("/health");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
