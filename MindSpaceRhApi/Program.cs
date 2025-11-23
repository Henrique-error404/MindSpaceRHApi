using Microsoft.EntityFrameworkCore;
using MindSpaceRhApi.Data;
using MindSpaceRhApi.Models;
using MindSpaceRhApi.Repositories;
using Oracle.EntityFrameworkCore;

using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuração do EF Core e Banco de Dados (SQL Server LocalDB) ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- 2. Injeção de Dependência (Padrão Repository) ---
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

// --- 3. Configuração do Swagger/OpenAPI (Documentação) ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 4. Configuração do Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- 5. Definição dos Endpoints (Minimal API) ---

// Endpoints CRUD para DEPARTMENTS (Setores)
app.MapGet("/api/departments", async (IDepartmentRepository repo) =>
    Results.Ok(await repo.GetAllAsync())).WithOpenApi();

app.MapGet("/api/departments/{id}", async (Guid id, IDepartmentRepository repo) =>
{
    var department = await repo.GetByIdAsync(id);
    return department != null ? Results.Ok(department) : Results.NotFound();
}).WithOpenApi();

app.MapPost("/api/departments", async (Department department, AppDbContext context) =>
{
    // AÇÃO CORRETIVA: Usar Where().FirstOrDefaultAsync() para evitar a tradução confusa do AnyAsync
    var existingDepartment = await context.Departments
        .Where(d => d.Name == department.Name)
        .FirstOrDefaultAsync(); // Retorna o primeiro ou nulo (mais explícito)

    if (existingDepartment != null) // Verifica se o resultado não é nulo
    {
        return Results.Conflict($"Setor com o nome '{department.Name}' já existe.");
    }

    // ... (restante da inserção)
    await context.Departments.AddAsync(department);
    await context.SaveChangesAsync();
    return Results.Created($"/api/departments/{department.Id}", department);
})
.WithOpenApi();

app.MapPut("/api/departments/{id}", async (Guid id, Department updatedDepartment, IDepartmentRepository repo, AppDbContext context) =>
{
    var existingDepartment = await repo.GetByIdAsync(id);
    if (existingDepartment == null) return Results.NotFound();

    existingDepartment.Name = updatedDepartment.Name;
    existingDepartment.EmployeeCount = updatedDepartment.EmployeeCount;

    await repo.UpdateAsync(existingDepartment);
    await context.SaveChangesAsync();
    return Results.NoContent();
}).WithOpenApi();

app.MapDelete("/api/departments/{id}", async (Guid id, IDepartmentRepository repo, AppDbContext context) =>
{
    var department = await repo.GetByIdAsync(id);
    if (department == null) return Results.NotFound();

    await repo.DeleteAsync(department);
    await context.SaveChangesAsync();
    return Results.NoContent();
}).WithOpenApi();


// --- REQUISITO ESPECIAL: GET /search (Paginação e Filtro) ---
app.MapGet("/api/departments/search", async (
    [FromQuery] string? nameFilter,
    [FromQuery] int page,
    [FromQuery] int size,
    IDepartmentRepository repo) =>
{
    if (page < 1) page = 1;
    if (size < 1) size = 10;

    var result = await repo.SearchDepartmentsAsync(nameFilter, page, size);

    return Results.Ok(result);
})
.WithOpenApi(operation => {
    operation.Summary = "Busca setores com Paginação e Filtro por Nome";
    operation.Description = "Permite filtrar setores por nome parcial e limitar os resultados.";
    return operation;
});


app.Run();