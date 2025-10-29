using DataAccess.DbContext.Data;
using Domain;
using Interfaces.Repositories;
using Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class ClientService : IClientService
    {

        private readonly IClient _clientRepository;
        private readonly ILogger<ClientService> _logger;
        public ClientService(IClient clientRepository, ILogger<ClientService> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }


        public List<TbClient> GetAllClients()
        {
            return _clientRepository.GetAllClients();
        }
        public TbClient GetClientById(int clientId)
        {
            return _clientRepository.GetAllClients().FirstOrDefault(c => c.ClientId == clientId);
        }

        public TbClient FindByAccountNumber(string accountNumber)
        {
            return _clientRepository.FindByAccountNumber(accountNumber);
        }

        public void Add(TbClient client) => _clientRepository.Add(client);

        public void Update(TbClient client) => _clientRepository.Update(client);
        public void ClientDelete(int clientId)
        {
            var client = _clientRepository.GetAllClients().FirstOrDefault(c => c.ClientId == clientId);
            if (client != null)
                _clientRepository.ClientDelete(client);
        }

    }
}
