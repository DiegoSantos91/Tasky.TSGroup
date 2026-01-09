# 📝 Tasky - Aplicación de Gestión de Tareas

Sistema de gestión de tareas construido con **Clean Architecture**, **ASP.NET Core 9** y **React**.

## 🏗️ Arquitectura

Este proyecto implementa **Clean Architecture** con principios de **Arquitectura Hexagonal**:

```
├── src/
│   ├── Tasky.Domain         # Entidades de negocio y enums
│   ├── Tasky.Application    # Casos de uso, DTOs, interfaces
│   ├── Tasky.Infrastructure # Acceso a datos, repositorios
│   ├── Tasky.Api           # Endpoints de la API REST
│   └── Tasky.Web           # Frontend en React
├── tests/
│   └── Tasky.Application.Tests
└── db/
    └── Scripts de migración SQL
```

## 🚀 Inicio Rápido (Quick Start)

### Prerrequisitos
- .NET 9 SDK
- Docker Desktop
- Node.js 20+

### Corriendo la Aplicación

**Opción 1: Usando Docker Compose (Recomendado)**
```bash
docker-compose up -d
```

**Opción 2: Corriendo Localmente**

1. **Levantar SQL Server**
Asegúrate de que Docker Desktop esté corriendo.
```bash
docker-compose up db -d
```

2. **Configurar Base de Datos (Solo la primera vez)**
Dado que es la primera vez que se ejecuta, necesitas crear el esquema de la base de datos.
*Nota: Si tienes problemas de esquema, puedes tener que aplicar migraciones manualmente. Esto usualmente solo es necesario en la configuración inicial o tras cambios en el esquema.*
```bash
dotnet ef database update --project src/Tasky.Infrastructure/Tasky.Infrastructure.csproj --startup-project src/Tasky.Api/Tasky.Api.csproj
```

3. **Correr la API Backend**
```bash
dotnet run --project src/Tasky.Api/Tasky.Api.csproj
```
La API estará disponible en `http://localhost:5000` con la UI de Swagger en la raíz.

4. **Correr el Frontend**
Abre una nueva terminal:
```bash
cd src/Tasky.Web
npm install
npm run dev
```
El Frontend estará disponible en `http://localhost:5173`.

### Configuración de Base de Datos

La base de datos se crea automáticamente al correr la API por primera vez si ejecutas el comando de migración del paso 2.

**Cadena de Conexión (Connection String):**
```
Server=localhost,1433;Database=TaskyDb;User Id=sa;Password=Tasky@2026!;TrustServerCertificate=True;
```

## 📚 Endpoints de la API

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/tasks` | Obtener todas las tareas |
| GET | `/api/tasks/{id}` | Obtener tarea por ID |
| POST | `/api/tasks` | Crear nueva tarea |
| PUT | `/api/tasks/{id}` | Actualizar tarea |
| PATCH | `/api/tasks/{id}/status` | Cambiar estado de tarea |
| DELETE | `/api/tasks/{id}` | Eliminar tarea |
| GET | `/health` | Chequeo de salud (Health check) |

## 🧪 Ejecutando Pruebas (Tests)

### Opción 1: Comando Estándar de .NET (Recomendado)
Este método funciona en cualquier terminal y no requiere permisos especiales.
```bash
dotnet test
```

### Opción 2: Usando Scripts de Ayuda
El proyecto incluye scripts de PowerShell en la carpeta `scripts/`. Si encuentras errores de permisos, necesitas permitir la ejecución de scripts para la sesión actual:

1. **Permitir ejecución de scripts (Solo sesión actual):**
```powershell
Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
```

2. **Correr el script de tests:**
```powershell
./scripts/run-tests.ps1
```

**Resultados de Pruebas:**
- ✅ 11 pruebas pasando
- ✅ 100% cobertura en TaskService

## 🛠️ Stack Tecnológico

### Backend
- **Framework:** ASP.NET Core 9
- **Base de Datos:** SQL Server 2022
- **ORM:** Entity Framework Core 9
- **Validación:** FluentValidation
- **Logging:** Serilog
- **Documentación:** Swagger/OpenAPI
- **Testing:** xUnit, Moq, FluentAssertions

### Frontend
- **Framework:** React 18 + Vite
- **Estilos:** Vanilla CSS con diseño moderno
- **Estado:** React Hooks
- **Cliente API:** Fetch API

### DevOps
- **Contenerización:** Docker & Docker Compose
- **Migraciones BD:** EF Core Migrations

## 🎨 Características

- ✅ Crear, Leer, Actualizar, Eliminar tareas (CRUD)
- ✅ Gestión de estados (Pendiente, En Progreso, Completada)
- ✅ Filtrar tareas por estado
- ✅ Edición de tareas en línea (Inline editing)
- ✅ Actualizaciones de UI en tiempo real
- ✅ Diseño moderno y responsivo
- ✅ API RESTful con documentación Swagger
- ✅ Logging estructurado
- ✅ Chequeos de salud (Health checks)
- ✅ Validación de entradas
- ✅ Manejo de errores global

## 📁 Estructura del Proyecto

### Capa de Dominio (`Tasky.Domain`)
- Entidades: `TaskItem`
- Enums: `TaskStatus`
- Sin dependencias de otras capas

### Capa de Aplicación (`Tasky.Application`)
- Servicios: `TaskService`
- Interfaces: `ITaskRepository`
- DTOs: `TaskDto`, `CreateTaskDto`, `UpdateTaskDto`, `ChangeStatusDto`
- Validadores: Reglas de FluentValidation

### Capa de Infraestructura (`Tasky.Infrastructure`)
- DbContext: `TaskyDbContext`
- Repositorios: `TaskRepository`
- Configuraciones: Mapeos de EF Core
- Configuración de Inyección de Dependencias

### Capa de API (`Tasky.Api`)
- Controladores: `TasksController`
- Configuración de Middleware
- Configuración de Swagger
- Políticas CORS

### Capa Web (`Tasky.Web`)
- Componentes: `TaskForm`, `TaskList`, `TaskItem`, `TaskFilters`
- Servicios: `taskApi`
- UI moderna con glassmorphism y animaciones

## 🔧 Configuración

### Backend (`appsettings.json`)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=TaskyDb;..."
  }
}
```

### Frontend (`.env`)
```
VITE_API_BASE_URL=http://localhost:5000
```

## 🐳 Docker

El proyecto incluye una configuración completa de Docker Compose.

```bash
# Iniciar todos los servicios
docker-compose up -d

# Detener todos los servicios
docker-compose down

# Ver logs de la base de datos
docker logs tasky-db
```

## 📊 Esquema de Base de Datos

**Tabla Tasks:**
- `Id` (GUID, PK, Identity)
- `Title` (nvarchar(200), requerido)
- `Description` (nvarchar(1000), nullable)
- `Status` (nvarchar(20), requerido) - Guardado como string enum
- `CreatedAt` (datetime2, requerido)
- `UpdatedAt` (datetime2, requerido)

## 🎯 Principios de Diseño

- **Clean Architecture:** Clara separación de responsabilidades
- **Principios SOLID:** Aplicados en todo el código
- **DRY:** No duplicar código
- **Patrón Repositorio:** Abstracción del acceso a datos
- **Inyección de Dependencias:** Bajo acoplamiento
- **Async/Await:** Todas las operaciones I/O son asíncronas

## 📝 Logging

Logging estructurado con Serilog:
- Salida por consola para desarrollo
- Salida a archivo: `logs/tasky-YYYYMMDD.txt`
- Rotación automática
- Para ver logs en tiempo real desde PowerShell:
  `Get-Content d:\tasky_migration\src\Tasky.Api\logs\tasky-*.txt -Wait -Tail 20`

## 🔒 Política CORS

La configuración actual permite todos los orígenes para desarrollo.
**Producción:** Configurar orígenes específicos en `Program.cs`.

## 🎨 Características UI

- **Diseño Moderno:** Fondos con gradientes, efectos glassmorphism
- **Responsivo:** Funciona en escritorio, tablet y móvil
- **Animaciones:** Transiciones suaves y micro-interacciones
- **Tema Oscuro:** Esquema de colores amigable a la vista
- **UX Intuitiva:** Feedback visual claro para todas las acciones

## 📄 Licencia

MIT License - ¡Siéntete libre de usar esto como plantilla para tus proyectos!