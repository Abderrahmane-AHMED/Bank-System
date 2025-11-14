using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface IEmployee
    {
        TbEmployee? GetById(int id);
        TbEmployee? GetByUsername(string username);
        IEnumerable<TbEmployee> GetAll();
        void Add(TbEmployee employee);
        void Update(TbEmployee employee);
        void Delete(int id);
    }
}
