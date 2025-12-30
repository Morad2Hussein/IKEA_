

namespace Demo.BLL.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync(string? DepartmentSearchName = null, bool withTracking = false);
        Task<DepartmentDetailsDto?> GetDepartmentByIdAsync(int id);
        Task<int> AddDepartmentAsync(CreateDepartmentDto departmentDto);
        Task<int> UpdateDepartmentAsync(UpdateDepartmentDto departmentDto);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}