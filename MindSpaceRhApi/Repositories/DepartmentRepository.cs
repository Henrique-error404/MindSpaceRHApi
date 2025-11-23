using Microsoft.EntityFrameworkCore;
using MindSpaceRhApi.Data;
using MindSpaceRhApi.Models;

namespace MindSpaceRhApi.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context) => _context = context;

        // CRUD Simples (Métodos Assíncronos)
        public async Task<List<Department>> GetAllAsync() => await _context.Departments.ToListAsync();
        public async Task<Department?> GetByIdAsync(Guid id) => await _context.Departments.FindAsync(id);
        public async Task AddAsync(Department department) => await _context.Departments.AddAsync(department);
        public Task UpdateAsync(Department department)
        {
            _context.Entry(department).State = EntityState.Modified;
            return Task.CompletedTask;
        }
        public Task DeleteAsync(Department department)
        {
            _context.Departments.Remove(department);
            return Task.CompletedTask;
        }

        // REQUISITO ESPECIAL: PAGINAÇÃO E FILTRO
        public async Task<PaginatedResult<Department>> SearchDepartmentsAsync(string? nameFilter, int page, int size)
        {
            var query = _context.Departments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                query = query.Where(d => d.Name.ToLower().Contains(nameFilter.ToLower()));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / size);

            var items = await query
                .OrderBy(d => d.Name)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return new PaginatedResult<Department>
            {
                Items = items,
                TotalPages = totalPages,
                TotalItems = totalItems,
                CurrentPage = page
            };
        }
    }
}