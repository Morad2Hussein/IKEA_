using Demo.BLL.DTOS.EmployeeDTOS;
using Demo.BLL.DTOS.EmployeeDTOS.Common;
using Demo.BLL.Services.Interfaces;
using Demo.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
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

        #region Get All Date
        [HttpGet]
        public IActionResult Index(string? EmployeeSearchName)
        {
            var employees = _employeeServices.GetAllEmployee(EmployeeSearchName);
            return View(employees);

        }

        #endregion
        #region Get Data By Id 
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Employee ID is required.");
            }
            var employee = _employeeServices.GetEmployeeById(id.Value);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        #endregion
        #region Create Employee Controller
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel createEmployee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _employeeServices.CreateEmployee(new CreateEmployeeDTO()
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
                    });
                    if (result > 0)
                        return RedirectToAction("Index");
                    else
                        ModelState.AddModelError("", "Failed to create employee. Please try again.");

                }
                catch (Exception ex)
                {

                    if (_webHostEnvironment.IsDevelopment())
                    {
                        _logger.LogError("An error occurred while creating the  : {Message}", ex.Message);

                    }
                    else
                    {
                        _logger.LogError($"An error occurred while creating the Employee: {ex.Message}|");
                        return View("ErrorView");

                    }
                }
            }
            return View("Create", createEmployee);
        }

        #endregion
        #region Edit Employees
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null || id < 0)
                return BadRequest($"Employee wtih this ID {id} not valid.");
            var employee = _employeeServices.GetEmployeeById(id.Value);
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
               

            };
            return View(employeeEdit);
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int? id, EmployeeViewModel updateEmployee)
        {
            if (id is null || id < 0 || id != updateEmployee.Id)
                return BadRequest($"Employee wtih this ID {id} not valid.");
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _employeeServices.UpdateEmployee(new UpdateEmployeeDto()
                    {
                        Id = id.Value,
                        Name = updateEmployee.Name,
                        Email = updateEmployee.Email,
                        Address = updateEmployee.Address,
                        PhoneNumber = updateEmployee.PhoneNumber,
                        Age= updateEmployee.Age,
                        Salary =    updateEmployee.Salary,
                        IsActive = updateEmployee.IsActive,
                        EmployeeType = updateEmployee.EmployeeType,
                        Gender = updateEmployee.Gender,
                        HiringDate = updateEmployee.HiringDate,
                        DepartmentId = updateEmployee.DepartmentId,
                    });
                    if (result > 0)
                        return RedirectToAction("Index");
                    else
                        ModelState.AddModelError("", "Failed to Update employee. Please try again.");
                }
                catch (Exception ex)
                {
                    if (_webHostEnvironment.IsDevelopment())
                    {
                        _logger.LogError("An error occurred while Updating the  : {Message}", ex.Message);
                    }
                    else
                    {
                        _logger.LogError($"An error occurred while Updating the Employee: {ex.Message}|");
                        return View("ErrorView");
                    }
                }
            }
            return View("Edit", updateEmployee);
        }
        #endregion
        #region Delete Employee
        [HttpPost]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
                return BadRequest();

            try
            {
                bool isDeleted = _employeeServices.DeleteEmployee(id);

                if (isDeleted)
                    return RedirectToAction("Index");
                else
                {
                    ModelState.AddModelError("", "Failed to delete employee. Please try again.");
                    return RedirectToAction("Index"); // أو عرض رسالة على نفس الصفحة
                }
            }
            catch (Exception ex)
            {
                if (_webHostEnvironment.IsDevelopment())
                {
                    _logger.LogError("An error occurred while deleting: {Message}", ex.Message);
                    ModelState.AddModelError("", "An error occurred. Try again.");
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError($"Error while deleting: {ex.Message}");
                    return View("ErrorView");
                }
            }
        }

        #endregion
    }
}
