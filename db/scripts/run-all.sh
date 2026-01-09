#!/bin/bash
# Script para ejecutar los scripts SQL en SQL Server

# Configuración
SERVER="localhost,1433"
USERNAME="sa"
PASSWORD="Tasky@2026!"

echo "============================================"
echo "Ejecutando scripts SQL para Tasky DB"
echo "============================================"
echo ""

# Verificar que sqlcmd está disponible
if ! command -v sqlcmd &> /dev/null
then
    echo "Error: sqlcmd no está instalado o no está en el PATH"
    echo "Intentando usar Docker para ejecutar los scripts..."
    
    # Ejecutar usando Docker
    echo ""
    echo "Ejecutando 01-create-database.sql..."
    docker exec -i tasky-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$PASSWORD" -C -i /scripts/01-create-database.sql
    
    echo ""
    echo "Ejecutando 02-create-tables.sql..."
    docker exec -i tasky-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$PASSWORD" -C -i /scripts/02-create-tables.sql
    
    echo ""
    echo "Ejecutando 03-seed-data.sql..."
    docker exec -i tasky-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$PASSWORD" -C -i /scripts/03-seed-data.sql
else
    # Ejecutar usando sqlcmd local
    echo "Ejecutando 01-create-database.sql..."
    sqlcmd -S $SERVER -U $USERNAME -P $PASSWORD -C -i 01-create-database.sql
    
    echo "Ejecutando 02-create-tables.sql..."
    sqlcmd -S $SERVER -U $USERNAME -P $PASSWORD -C -i 02-create-tables.sql
    
    echo "Ejecutando 03-seed-data.sql..."
    sqlcmd -S $SERVER -U $USERNAME -P $PASSWORD -C -i 03-seed-data.sql
fi

echo ""
echo "============================================"
echo "Scripts ejecutados exitosamente"
echo "============================================"
