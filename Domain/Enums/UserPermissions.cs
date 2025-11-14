using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    [Flags]
    public enum UserPermissions
    {
        None = 0,
        All = -1,
        ListClients = 1,
        NewClient = 2,
        UpdateClients = 4,
        DeleteClient = 8,
        Transactions = 16,
        FindClient = 32,
        ManageUsers = 64,
       
    }
   
}
