# Apply migrations to database
Write-Host "🗄️ Applying migrations..." -ForegroundColor Cyan

dotnet ef database update `
    -p src/Tasky.Infrastructure/Tasky.Infrastructure.csproj `
    -s src/Tasky.Api/Tasky.Api.csproj `
    --context Tasky.Infrastructure.Persistence.TaskyDbContext

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Database updated!" -ForegroundColor Green
}
else {
    Write-Host "❌ Update failed!" -ForegroundColor Red
    exit 1
}
