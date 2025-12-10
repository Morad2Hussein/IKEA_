using Demo.BLL.DTOS.EmployeeDTOS;

namespace Demo.BLL.Services.Interfaces
{
    public interface IEmployeeServices
    {
        // Get All Employees
        IEnumerable<EmployeeDto> GetAllEmployee( string? EmployeeSearchName,bool withTracking = false);

        // Get Employee By Id
        EmployeeDetailsDTO? GetEmployeeById(int id);
        // Create Employee
        int CreateEmployee(CreateEmployeeDTO createEmployee);
        // Update Employee
        int UpdateEmployee(UpdateEmployeeDto updateEmployee);

        // Delete Employee
         bool DeleteEmployee(int id);
    }
}
