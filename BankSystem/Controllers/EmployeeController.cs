using BusinessLogic.Services;
using Domain;
using Domain.Enums;
using Interfaces.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class EmployeeController : Controller
    {
        

      
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<ClientController> _logger;
        public EmployeeController(IEmployeeService employeeService, ILogger<ClientController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var employees = _employeeService.GetAll();
            return View(employees);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(string username, string password, int[] permissions)
        {
            UserPermissions finalPerm = 0;
            foreach (var p in permissions)
                finalPerm |= (UserPermissions)p;

            _employeeService.CreateEmployee(username, password, finalPerm);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var emp = _employeeService.GetById(id);
            if (emp == null) return NotFound();
            return View(emp);
        }

        [HttpPost]
        public IActionResult Edit(TbEmployee employee, int[] permissions, string? newPassword)
        {
            var oldEmp = _employeeService.GetById(employee.Id);
            if (oldEmp == null) return NotFound();

            
            oldEmp.Username = employee.Username;

           
            UserPermissions finalPerm = 0;
            foreach (var p in permissions)
                finalPerm |= (UserPermissions)p;
            oldEmp.Permissions = finalPerm;

         
            if (!string.IsNullOrEmpty(newPassword))
            {
                oldEmp.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            }

            _employeeService.UpdateEmployee(oldEmp);

            return RedirectToAction("Index");
        }



        public IActionResult Delete(int id)
        {
            _employeeService.DeleteEmployee(id);
            return RedirectToAction("Index");
        }
    }

}
