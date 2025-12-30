using Demo.BLL.DTOS.EmployeeDTOS;

namespace Demo.BLL.Services.Interfaces
{
    public interface IEmployeeServices
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeeAsync(string? EmployeeSearchName, bool withTracking = false);
        Task<EmployeeDetailsDTO?> GetEmployeeByIdAsync(int id);
        Task<int> CreateEmployeeAsync(CreateEmployeeDTO createEmployee);
        Task<int> UpdateEmployeeAsync(UpdateEmployeeDto updateEmployee);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}