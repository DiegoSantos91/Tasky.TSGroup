# Run all tests
Write-Host "🧪 Running tests..." -ForegroundColor Cyan

dotnet test --logger "console;verbosity=detailed"

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ All tests passed!" -ForegroundColor Green
}
else {
    Write-Host "❌ Tests failed!" -ForegroundColor Red
    exit 1
}
