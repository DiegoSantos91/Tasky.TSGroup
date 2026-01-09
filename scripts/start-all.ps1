# Tasky Development Helper Scripts

# Start all services with Docker Compose
docker-compose up -d

Write-Host "✅ All services started!" -ForegroundColor Green
Write-Host "📊 SQL Server: localhost:1433" -ForegroundColor Cyan
Write-Host "🚀 API: http://localhost:5000" -ForegroundColor Cyan
Write-Host "🎨 Frontend: http://localhost:5173" -ForegroundColor Cyan
Write-Host ""
Write-Host "View logs with: docker-compose logs -f" -ForegroundColor Yellow
