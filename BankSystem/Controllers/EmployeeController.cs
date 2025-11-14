using BusinessLogic.Services;
using Domain;
using Domain.Enums;
using Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
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
        public IActionResult Edit(TbEmployee employee, int[] permissions)
        {
            UserPermissions finalPerm = 0;
            foreach (var p in permissions)
                finalPerm |= (UserPermissions)p;

            employee.Permissions = finalPerm;
            _employeeService.UpdateEmployee(employee);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _employeeService.DeleteEmployee(id);
            return RedirectToAction("Index");
        }
    }

}
