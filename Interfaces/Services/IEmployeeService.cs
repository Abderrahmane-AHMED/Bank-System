using Domain;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Services
{
    public interface IEmployeeService
    {
        TbEmployee? GetById(int id);
        TbEmployee? GetByUsername(string username);
        IEnumerable<TbEmployee> GetAll();
        void CreateEmployee(string username, string password, UserPermissions permissions);
        void UpdateEmployee(TbEmployee employee);
        void DeleteEmployee(int id);
    }
}
