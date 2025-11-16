using BankSystem.Models;
using BankSystem.ViewModels;
using Domain;
using Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IClientService _clientService;
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IEmailSender emailSender , IClientService clientService, IEmployeeService employeeService, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _clientService = clientService;
            _employeeService = employeeService;
            _logger = logger;
        }

        #region Login/Logout

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string identifier, string password, string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(identifier) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Incorrect Username/Email or Password.");
                return View();
            }

            // ======== 1- محاولة تسجيل الدخول كـ ApplicationUser ========
            // المستخدمين العاديين يسجلون بـ Email
            var user = await _userManager.FindByEmailAsync(identifier);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, password, true, false);

                if (result.Succeeded)
                {
                    // مستخدم عادي
                    return Redirect(returnUrl ?? "/");
                }
            }

            // ======== 2- محاولة تسجيل الدخول كموظف Employee ========
            // الموظفون يسجلون بـ Username
            var emp = _employeeService.GetByUsername(identifier);

            if (emp != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(password, emp.PasswordHash))
                {
                    ModelState.AddModelError("", "Incorrect Username/Email or Password.");
                    return View();
                }

                // إنشاء Claims للموظف
                var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, emp.Username),
    new Claim("EmployeeId", emp.Id.ToString()),
    new Claim("Permissions", ((int)emp.Permissions).ToString()),
    new Claim("IsEmployee", "true")
};

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                    });


                // توجيه الموظف لصفحة النظام الخاصة بهم
                return Redirect("/Application/Index");
            }

            // ======== فشل الدخول ========
            ModelState.AddModelError("", "Incorrect Username/Email or Password.");
            return View();
        }


        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        #endregion

      

        #region Forgot/Reset Password

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email)) return View();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return RedirectToAction("ForgotPasswordConfirmation");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { email, token }, Request.Scheme);
            await _emailSender.SendEmailAsync(email, "Reset Password", $"Click here to reset your password: <a href='{resetLink}'>link</a>");

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation() => View();

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string token) =>
            View(new ResetPasswordViewModel { Email = email, Token = token });

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return RedirectToAction("ResetPasswordConfirmation");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded) return RedirectToAction("ResetPasswordConfirmation");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation() => View();

        #endregion


        #region ConfirmEmailSuccess

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return RedirectToAction("Index", "Home");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return View("ConfirmEmail");
            else
                return View("Error");
        }

        #endregion


        #region UpdateConfirmation
        public IActionResult UpdateConfirmation(int clientId)
        {
            var client = _clientService.GetClientById(clientId);
            if (client == null)
            {
                TempData["ErrorMessage"] = "Client not found.";
                return RedirectToAction("ErrorPage");
            }
            return View(client);
        }
        #endregion



        [HttpGet]
        public IActionResult AccessDenied() => View();
    }
}
