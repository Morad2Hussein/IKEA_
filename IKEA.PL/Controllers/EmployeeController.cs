using Demo.BLL.DTOS.EmployeeDTOS;
using Demo.BLL.DTOS.EmployeeDTOS.Common;
using Demo.BLL.Services.Interfaces;
using Demo.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeServices _employeeServices;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(IEmployeeServices employeeServices, ILogger<EmployeeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _employeeServices = employeeServices;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Get All Data
        [HttpGet]
        public async Task<IActionResult> Index(string? EmployeeSearchName)
        {
            // Await the async service call
            var employees = await _employeeServices.GetAllEmployeeAsync(EmployeeSearchName);
            return View(employees);
        }
        #endregion

        #region Get Data By Id 
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return BadRequest("Employee ID is required.");

            // Await the async service call
            var employee = await _employeeServices.GetEmployeeByIdAsync(id.Value);

            if (employee == null)
                return NotFound();

            return View(employee);
        }
        #endregion

        #region Create Employee Controller
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel createEmployee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Await the async creation
                    var result = await _employeeServices.CreateEmployeeAsync(new CreateEmployeeDTO()
                    {
                        Name = createEmployee.Name,
                        Email = createEmployee.Email,
                        Address = createEmployee.Address,
                        PhoneNumber = createEmployee.PhoneNumber,
                        Age = createEmployee.Age,
                        Salary = createEmployee.Salary,
                        IsActive = createEmployee.IsActive,
                        EmployeeType = createEmployee.EmployeeType,
                        Gender = createEmployee.Gender,
                        HiringDate = createEmployee.HiringDate,
                        DepartmentId = createEmployee.DepartmentId,
                        Photo = createEmployee.Photo,
                    });

                    if (result > 0)
                        return RedirectToAction(nameof(Index));

                    ModelState.AddModelError("", "Failed to create employee. Please try again.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while creating the Employee");
                    if (!_webHostEnvironment.IsDevelopment()) return View("ErrorView");
                    throw; // Re-throw in dev for the developer exception page
                }
            }
            return View("Create", createEmployee);
        }
        #endregion

        #region Edit Employees
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null || id < 0)
                return BadRequest($"Employee with ID {id} is not valid.");

            var employee = await _employeeServices.GetEmployeeByIdAsync(id.Value);

            if (employee is null)
                return NotFound();

            var employeeEdit = new EmployeeViewModel()
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                Address = employee.Address,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId,
                IsActive = employee.IsActive,
                EmployeeType = Enum.Parse<EmployeeType>(employee.EmployeeTypes!),
                Gender = Enum.Parse<Gender>(employee.Gender!),
                PhotoName = employee.PhotoName
            };
            return View(employeeEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int? id, EmployeeViewModel updateEmployee)
        {
            if (id is null || id != updateEmployee.Id)
                return BadRequest("ID Mismatch");

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _employeeServices.UpdateEmployeeAsync(new UpdateEmployeeDto()
                    {
                        Id = id.Value,
                        Name = updateEmployee.Name,
                        Email = updateEmployee.Email,
                        Address = updateEmployee.Address,
                        PhoneNumber = updateEmployee.PhoneNumber,
                        Age = updateEmployee.Age,
                        Salary = updateEmployee.Salary,
                        IsActive = updateEmployee.IsActive,
                        EmployeeType = updateEmployee.EmployeeType,
                        Gender = updateEmployee.Gender,
                        HiringDate = updateEmployee.HiringDate,
                        DepartmentId = updateEmployee.DepartmentId,
                        Photo = updateEmployee.Photo,
                    });

                    if (result > 0)
                        return RedirectToAction(nameof(Index));

                    ModelState.AddModelError("", "Failed to Update employee.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating employee");
                    if (!_webHostEnvironment.IsDevelopment()) return View("ErrorView");
                }
            }
            return View("Edit", updateEmployee);
        }
        #endregion

        #region Delete Employee
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0) return BadRequest();

            try
            {
                bool isDeleted = await _employeeServices.DeleteEmployeeAsync(id);

                if (isDeleted)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Failed to delete employee.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee");
                return View("ErrorView");
            }
        }
        #endregion
    }
}