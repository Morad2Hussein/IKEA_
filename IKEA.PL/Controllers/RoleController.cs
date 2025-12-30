using BLL.ViewModels.IdentityModels.Role_odels;
using DAL.Models.IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IKEA.PL.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        // 1. Add this line
        private readonly ILogger<RoleController> _logger;

        // 2. Add ILogger to the constructor parameters
        public RoleController(
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager,
            ILogger<RoleController> logger) // <--- Add here
        {
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _logger = logger; // <--- Assign here
        }

        #region Index
        [HttpGet]
        public async Task<IActionResult> Index(string? search)
        {
            var rolesQuery = _roleManager.Roles;

            if (!string.IsNullOrEmpty(search))
            {
                rolesQuery = rolesQuery.Where(r => r.Name!.ToLower().Contains(search.ToLower()));
            }

            // Using await with ToListAsync for better performance
            var rolesList = await rolesQuery.Select(r => new RoleViewModels()
            {
                Id = r.Id,
                Name = r.Name!
            }).ToListAsync();

            return View(rolesList);
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var roleViewModel = new RoleViewModels
            {
                Id = role.Id,
                Name = role.Name!
            };
            return View(roleViewModel);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var allUsers = await _userManager.Users.ToListAsync();

            var roleViewModel = new RoleViewModels
            {
                Id = role.Id,
                Name = role.Name!,
                users = new List<UserRoleViewModels>()
            };

            foreach (var user in allUsers)
            {
                roleViewModel.users.Add(new UserRoleViewModels
                {
                    UserId = user.Id,
                    UserName = user.UserName!,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name!)
                });
            }

            return View(roleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, RoleViewModels roleViewModel)
        {
            if (id != roleViewModel.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    if (role == null) return NotFound();

                    role.Name = roleViewModel.Name;
                    var updateResult = await _roleManager.UpdateAsync(role);

                    if (updateResult.Succeeded)
                    {
                        foreach (var userRole in roleViewModel.users)
                        {
                            var user = await _userManager.FindByIdAsync(userRole.UserId);
                            if (user != null)
                            {
                                bool currentlyInRole = await _userManager.IsInRoleAsync(user, role.Name!);

                                if (userRole.IsSelected && !currentlyInRole)
                                {
                                    await _userManager.AddToRoleAsync(user, role.Name!);
                                }
                                else if (!userRole.IsSelected && currentlyInRole)
                                {
                                    await _userManager.RemoveFromRoleAsync(user, role.Name!);
                                }
                            }
                        }
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in updateResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating role");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
                }
            }

            return View(roleViewModel);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModels roleViewModel)
        {
            if (!ModelState.IsValid) return View(roleViewModel);

            var result = await _roleManager.CreateAsync(new IdentityRole { Name = roleViewModel.Name });

            if (result.Succeeded) return RedirectToAction(nameof(Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(roleViewModel);
        }
        #endregion

        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded) return RedirectToAction(nameof(Index));

            TempData["Message"] = "Role could not be deleted.";
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}