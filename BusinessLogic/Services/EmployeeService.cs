
using DataAccess.Repository;
using Domain;
using Domain.Enums;                                            
using Interfaces;
using Interfaces.Repositories;
using Interfaces.Services;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto.Generators;

namespace BusinessLogic.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployee _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;
        public EmployeeService(IEmployee employeeRepository, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }


        public TbEmployee? GetById(int id) => _employeeRepository.GetById(id);

        public TbEmployee? GetByUsername(string username) => _employeeRepository.GetByUsername(username);

        public IEnumerable<TbEmployee> GetAll() => _employeeRepository.GetAll();

        public void CreateEmployee(string username, string password, UserPermissions permissions)
        {
            var emp = new TbEmployee
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Permissions = permissions
            };

            _employeeRepository.Add(emp);
        }

        public void UpdateEmployee(TbEmployee employee) => _employeeRepository.Update(employee);

        public void DeleteEmployee(int id) => _employeeRepository.Delete(id);
    }
}
