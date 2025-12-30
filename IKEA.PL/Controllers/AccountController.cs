using BLL.Services.EmailSetting;
using BLL.ViewModels.IdentityModels;
using DAL.Models.Common;
using DAL.Models.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IKEA.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSettings _emailSettings;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IEmailSettings emailSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSettings = emailSettings;
        }

        #region Register
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var user = new ApplicationUser
            {
                UserName = registerViewModel.Username,
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                Email = registerViewModel.Email,
            };

            // Use await instead of .Result
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
                return RedirectToAction("Login", "Account");

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(registerViewModel);
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginView)
        {
            if (!ModelState.IsValid) return View(loginView);

            var user = await _userManager.FindByEmailAsync(loginView.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(loginView);
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginView.Password);
            if (isPasswordValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, loginView.Password, loginView.RememberMe, false);

                if (signInResult.Succeeded)
                    return RedirectToAction("Index", "Home");

                if (signInResult.IsNotAllowed)
                    ModelState.AddModelError(string.Empty, "You are not allowed to login.");
                else if (signInResult.IsLockedOut)
                    ModelState.AddModelError(string.Empty, "Your account is locked out.");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(loginView);
        }
        #endregion

        #region LogOut
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region ForgetPassword
        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (!ModelState.IsValid) return View(nameof(ForgotPassword));

            var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var url = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { email = forgotPasswordViewModel.Email, token = token },
                    Request.Scheme
                );

                var email = new ResetEmail
                {
                    To = forgotPasswordViewModel.Email,
                    Subject = "Reset Your Password",
                    Body = url
                };

                // Await your new Async email service
                await _emailSettings.SendEmailAsync(email);
            }

            return RedirectToAction(nameof(CheckYourInBox));
        }

        [HttpGet]
        public IActionResult CheckYourInBox() => View();
        #endregion

        #region ResetPassword
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return BadRequest("Invalid reset request");

            var model = new ResetPasswordViewModel { Email = email, Token = token };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // For security, don't reveal user doesn't exist, but here we keep your logic
                ModelState.AddModelError("", "Invalid request");
                return View(model);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
                return RedirectToAction(nameof(Login));

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
        #endregion
    }
}