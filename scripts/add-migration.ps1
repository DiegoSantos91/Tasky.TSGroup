# Create a new migration
param(
    [Parameter(Mandatory = $true)]
    [string]$MigrationName
)

Write-Host "📝 Creating migration: $MigrationName" -ForegroundColor Cyan

dotnet ef migrations add $MigrationName `
    -p src/Tasky.Infrastructure/Tasky.Infrastructure.csproj `
    -s src/Tasky.Api/Tasky.Api.csproj `
    -o Data/Migrations `
    --context Tasky.Infrastructure.Persistence.TaskyDbContext

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Migration created!" -ForegroundColor Green
}
else {
    Write-Host "❌ Migration failed!" -ForegroundColor Red
    exit 1
}
