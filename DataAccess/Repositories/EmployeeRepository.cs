using DataAccess.DbContext.Data;
using Domain;
using Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class EmployeeRepository : IEmployee
    {
        private readonly BankSystemContext _context;
        private readonly ILogger<EmployeeRepository> _logger;
        public EmployeeRepository(BankSystemContext context, ILogger<EmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public TbEmployee? GetById(int id) => _context.Employees.Find(id);

        public TbEmployee? GetByUsername(string username) => _context.Employees.FirstOrDefault(e => e.Username == username);

        public IEnumerable<TbEmployee> GetAll() => _context.Employees.ToList();

        public void Add(TbEmployee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void Update(TbEmployee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var emp = _context.Employees.Find(id);
            if (emp != null)
            {
                _context.Employees.Remove(emp);
                _context.SaveChanges();
            }
        }
    }
}
