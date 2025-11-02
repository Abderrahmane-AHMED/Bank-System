using System;
using Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repositories
{
    public interface IClient
    {


        List<TbClient> GetAllClients();

        TbClient FindByAccountNumber(string accountNumber);

        void Add(TbClient client);
        void Update(TbClient client);

        void ClientDelete(TbClient clientId);

        void ClientDeposit(int clientId, decimal amount);

        void ClientWithdraw(int clientId, decimal amount);


    }
}
