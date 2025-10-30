using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    public class ApplicationController : Controller
    {

        public IActionResult index()
        {
            return View();
        }



    }
}
