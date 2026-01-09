# Build all projects
Write-Host "🔨 Building solution..." -ForegroundColor Cyan

dotnet build

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Build successful!" -ForegroundColor Green
}
else {
    Write-Host "❌ Build failed!" -ForegroundColor Red
    exit 1
}
