using MindSpaceRhApi.Models;

namespace MindSpaceRhApi.Repositories
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(Guid id);
        Task AddAsync(Department department);
        Task UpdateAsync(Department department);
        Task DeleteAsync(Department department);

        Task<PaginatedResult<Department>> SearchDepartmentsAsync(string? nameFilter, int page, int size);
    }

    // Classe de Estrutura para Resultados Paginados (Requisito de Paginação)
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
    }
}