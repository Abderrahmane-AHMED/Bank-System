using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Services
{
    public interface IClientService
    {
        TbClient GetClientById(int clientId);

        List<TbClient> GetAllClients();

        TbClient FindByAccountNumber(string accountNumber);
        void Add(TbClient client);
        void Update(TbClient client);
        void ClientDelete(int clientId);

        void ClientDeposit(int clientId, decimal amount);

        void ClientWithdraw(int clientId, decimal amount);


    }
}
