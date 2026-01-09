# Script PowerShell para ejecutar los scripts SQL en SQL Server

# Configuración
$Server = "localhost,1433"
$Username = "sa"
$Password = "Tasky@2026!"
$ScriptsPath = $PSScriptRoot

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Ejecutando scripts SQL para Tasky DB" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Verificar si el contenedor de Docker está corriendo
$dockerRunning = docker ps --filter "name=tasky-db" --format "{{.Names}}" 2>$null

if ($dockerRunning -eq "tasky-db") {
    Write-Host "Usando contenedor Docker 'tasky-db'..." -ForegroundColor Yellow
    Write-Host ""
    
    Write-Host "Ejecutando 01-create-database.sql..." -ForegroundColor Green
    Get-Content "$ScriptsPath\01-create-database.sql" | docker exec -i tasky-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U $Username -P $Password -C
    
    Write-Host ""
    Write-Host "Ejecutando 02-create-tables.sql..." -ForegroundColor Green
    Get-Content "$ScriptsPath\02-create-tables.sql" | docker exec -i tasky-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U $Username -P $Password -C
    
    Write-Host ""
    Write-Host "Ejecutando 03-seed-data.sql..." -ForegroundColor Green
    Get-Content "$ScriptsPath\03-seed-data.sql" | docker exec -i tasky-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U $Username -P $Password -C
}
elseif (Get-Command sqlcmd -ErrorAction SilentlyContinue) {
    Write-Host "Usando sqlcmd local..." -ForegroundColor Yellow
    Write-Host ""
    
    Write-Host "Ejecutando 01-create-database.sql..." -ForegroundColor Green
    sqlcmd -S $Server -U $Username -P $Password -C -i "$ScriptsPath\01-create-database.sql"
    
    Write-Host ""
    Write-Host "Ejecutando 02-create-tables.sql..." -ForegroundColor Green
    sqlcmd -S $Server -U $Username -P $Password -C -i "$ScriptsPath\02-create-tables.sql"
    
    Write-Host ""
    Write-Host "Ejecutando 03-seed-data.sql..." -ForegroundColor Green
    sqlcmd -S $Server -U $Username -P $Password -C -i "$ScriptsPath\03-seed-data.sql"
}
else {
    Write-Host "Error: No se encontró sqlcmd ni el contenedor Docker 'tasky-db'" -ForegroundColor Red
    Write-Host "Por favor, inicie el contenedor con 'docker-compose up -d' o instale SQL Server tools" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Scripts ejecutados exitosamente" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
