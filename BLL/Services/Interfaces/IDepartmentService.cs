using Demo.BLL.DTOS.DepartmentModule;

namespace Demo.BLL.Services.Interfaces
{
    public interface IDepartmentService
    {
        IEnumerable<DepartmentDto> GetAllDepartments(string? DepartmentSearchName = null , bool withTracking = false);
         DepartmentDetailsDto? GetDepartmentById(int id);
        int AddDepartment(CreateDepartmentDto departmentDto);
        int UpdateDepartment( UpdateDepartmentDto departmentDto);
        bool DeleteDepartment(int id);


    }
}
