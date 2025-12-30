using Demo.BLL.DTOS.DepartmentModule;
using Demo.BLL.Services.Interfaces;
using Demo.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(
                IDepartmentService departmentServices,
                IWebHostEnvironment webHostEnvironment,
                ILogger<DepartmentController> logger)
        {
            _departmentServices = departmentServices;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        #region Get all Data
        [HttpGet]
        public async Task<IActionResult> Index(string? DepartmentSearchName)
        {
            // Await the async service call
            var departments = await _departmentServices.GetAllDepartmentsAsync(DepartmentSearchName);
            return View(departments);
        }
        #endregion

        #region Create Department
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Recommended for POST actions
        public async Task<IActionResult> Create(CreateDepartmentDto createDepartment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Await the async creation
                    int result = await _departmentServices.AddDepartmentAsync(createDepartment);

                    if (result > 0)
                        TempData["Message"] = "Department created successfully.";
                    else
                        TempData["Message"] = "Failed to create department.";

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while creating the department");
                    if (!_webHostEnvironment.IsDevelopment()) return View("ErrorView");
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(createDepartment);
        }
        #endregion

        #region Department Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();

            var department = await _departmentServices.GetDepartmentByIdAsync(id);

            if (department == null) return NotFound();

            return View(department);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return BadRequest();

            var department = await _departmentServices.GetDepartmentByIdAsync(id);

            if (department == null) return NotFound();

            var departmentEdit = new DepartmentViewModel()
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                Description = department.Description,
                CreatedOn = department.CreatedOn ?? default
            };

            return View(departmentEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentViewModel departmentEdit)
        {
            if (id <= 0 || id != departmentEdit.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var updateDepartmentDto = new UpdateDepartmentDto()
                    {
                        Id = id,
                        Code = departmentEdit.Code,
                        Name = departmentEdit.Name,
                        Description = departmentEdit.Description,
                        DateOfCreation = departmentEdit.CreatedOn
                    };

                    int result = await _departmentServices.UpdateDepartmentAsync(updateDepartmentDto);

                    if (result > 0) return RedirectToAction(nameof(Index));

                    ModelState.AddModelError("", "Failed to update department.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during department update");
                    if (!_webHostEnvironment.IsDevelopment()) return View("ErrorView");
                }
            }

            return View(departmentEdit);
        }
        #endregion

        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            try
            {
                bool isDeleted = await _departmentServices.DeleteDepartmentAsync(id);

                if (isDeleted) return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Failed to delete department.");
                // Fetching data again to show the view with errors if delete failed
                var department = await _departmentServices.GetDepartmentByIdAsync(id);
                return View("Details", department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department");
                return View("ErrorView");
            }
        }
        #endregion
    }
}