# Check health of all services
Write-Host "🏥 Checking service health..." -ForegroundColor Cyan
Write-Host ""

# Check API
try {
    $apiResponse = Invoke-WebRequest -Uri "http://localhost:5000/health" -UseBasicParsing -TimeoutSec 5
    if ($apiResponse.StatusCode -eq 200) {
        Write-Host "✅ API is healthy" -ForegroundColor Green
    }
}
catch {
    Write-Host "❌ API is not responding" -ForegroundColor Red
}

# Check Frontend
try {
    $webResponse = Invoke-WebRequest -Uri "http://localhost:5173" -UseBasicParsing -TimeoutSec 5
    if ($webResponse.StatusCode -eq 200) {
        Write-Host "✅ Frontend is running" -ForegroundColor Green
    }
}
catch {
    Write-Host "❌ Frontend is not responding" -ForegroundColor Red
}

# Check Docker containers
Write-Host ""
Write-Host "Docker containers:" -ForegroundColor Cyan
docker ps --filter "name=tasky-" --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"
