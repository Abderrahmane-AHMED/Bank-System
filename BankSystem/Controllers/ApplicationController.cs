using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies,Identity.Application")]
    public class ApplicationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }



}
