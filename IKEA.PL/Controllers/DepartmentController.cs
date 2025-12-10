using AspNetCoreGeneratedDocument;
using Demo.BLL.DTOS.DepartmentModule;
using Demo.BLL.Services.Interfaces;
using Demo.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<DepartmentController> _logger;
        public DepartmentController(
                IDepartmentService departmentServices, IWebHostEnvironment webHostEnvironment , ILogger<DepartmentController> logger)
        {
            _departmentServices = departmentServices;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;   
        }
        #region Get all Data
        [HttpGet]
        public IActionResult Index(string? DepartmentSearchName)
        {
            var Departments = _departmentServices.GetAllDepartments(DepartmentSearchName);
            return View(Departments);
        }
        #endregion
        #region Create Department
        // get form to create department
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        // post form to create department
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(CreateDepartmentDto createDepartment)
        {
                if (ModelState.IsValid)
            {

            try
            {
                {
                    int result = _departmentServices.AddDepartment( new CreateDepartmentDto(){
                            Code = createDepartment.Code,
                                Name = createDepartment.Name,
                                Description = createDepartment.Description,
                                DateOfCreation = createDepartment.DateOfCreation
                    });
                        string message;
                    if (result > 0)
                           message = "Department created successfully.";
                        else
                         message = "Failed to create department. Please try again.";
                       TempData["Message"] = message;
                        return RedirectToAction(nameof(Index));

                }
            }
            catch (Exception ex)
            {

                    if (_webHostEnvironment.IsDevelopment())
                    {
                        _logger.LogError("An error occurred while creating the department: {Message}", ex.Message);
                    
                    }
                    else
                    {
                        _logger.LogError($"An error occurred while creating the department {ex.Message}|");
                        return View("ErrorView");

                    }

                }

            }
              
                    return View(createDepartment);
            
        }
        #endregion
        #region Department Details
        [HttpGet]
        public IActionResult Details(int id)
        {
          //  if(id.HasValue) return BadRequest();

            if (id <= 0)
            {
                return BadRequest();
            }
            var department = _departmentServices.GetDepartmentById(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        #endregion
        #region Edit
        //[HttpGet]
        //public IActionResult Edit(int id)
        //{
        //    if (id <= 0)
        //        return BadRequest();
        //    var department = _departmentServices.GetDepartmentById(id);
        //    if (department == null)
        //        return NotFound();
        //    var depatmentEdit = new DepartmentViewModel()
        //    {
        //        Id = department.Id,
        //        Code = department.Code,
        //        Name = department.Name,
        //        Description = department.Description,
        //        CreatedOn = department.CreatedOn.HasValue ? department.CreatedOn.Value : default
        //    };
        //    return View(depatmentEdit);
        //}
        //[HttpPost]
        //public IActionResult Edit(int id, DepartmentViewModel departmrntEdit)
        //{
        //    if (id <= 0)
        //        return BadRequest();
        //    if (ModelState.IsValid)
        //    {
        //        var updateDepartmentDto = new UpdateDepartmentDto()
        //        {
        //            Id = id,
        //            Code = departmrntEdit.Code,
        //            Name = departmrntEdit.Name,
        //            Description = departmrntEdit.Description,
        //            DateOfCreation = departmrntEdit.CreatedOn
        //        };
        //        int result = _departmentServices.UpdateDepartment(updateDepartmentDto);
        //        if (result > 0)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Failed to update department. Please try again.");
        //        }
        //    }
        //    return View(departmrntEdit);
        //}
        #endregion
        #region New Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id <= 0)
                return BadRequest();

            var department = _departmentServices.GetDepartmentById(id);
            if (department == null)
                return NotFound();

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
        public IActionResult Edit(int id, DepartmentViewModel departmentEdit)
        {
            if (id <= 0)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var updateDepartmentDto = new UpdateDepartmentDto()
                {
                    Id = id,
                    Code = departmentEdit.Code,
                    Name = departmentEdit.Name,
                    Description = departmentEdit.Description,
                    DateOfCreation = departmentEdit.CreatedOn
                };

                int result = _departmentServices.UpdateDepartment(updateDepartmentDto);

                if (result > 0)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Failed to update department. Please try again.");
            }

            return View(departmentEdit);
        }

        #endregion
        #region Delete
        [HttpGet]
        #region  delete if i want to see data  before deleted it 
    
        #endregion
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();
            bool isDeleted = _departmentServices.DeleteDepartment(id);
            if (isDeleted)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to delete department. Please try again.");
                var department = _departmentServices.GetDepartmentById(id);
                return View("Delete", department);
            }
        }
        #endregion
    }
}

