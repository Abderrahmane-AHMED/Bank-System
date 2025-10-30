


using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult index()
        {
            return View();
        }
    }
}
