using BankSystem.Models;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    public class UserController : Controller
    {

        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

       

        #region Register

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View(new UserModel());

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token }, Request.Scheme);
                await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Click here to confirm your email: <a href='{confirmationLink}'>Email confirmation</a>");
                return View("RegistrationConfirmation");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }



        #endregion




        [HttpGet]
        public IActionResult AccessDenied() => View();

    }
}
