using DataAccess.DbContext.Data;
using Domain;
using Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ClientRepository : IClient
    {
        private readonly BankSystemContext _context;
        private readonly ILogger<ClientRepository> _logger;
        public ClientRepository(BankSystemContext context, ILogger<ClientRepository> logger)
        {
            _context = context;
            _logger = logger;
        }




        public List<TbClient> GetAllClients()
        {
            return _context.Clients.ToList();
        }

        public TbClient FindByAccountNumber(string accountNumber)
        {
            return _context.Clients.FirstOrDefault(c => c.AccountNumber == accountNumber);
        }


        public void Add(TbClient client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }
        public void Update(TbClient client)
        {

            var existingClient = _context.Clients
                .FirstOrDefault(c => c.ClientId == client.ClientId);

            if (existingClient != null)
            {
                existingClient.FirstName = client.FirstName;
                existingClient.LastName = client.LastName;
                existingClient.PhoneNumber = client.PhoneNumber;
                existingClient.Balance = client.Balance;
                existingClient.PinCode = client.PinCode;

                _context.SaveChanges();

            }



        }


        public void ClientDelete(TbClient clientId)
        {
            _context.Clients.Remove(clientId);
            _context.SaveChanges();

        }

 
        public void ClientDeposit(int clientId, decimal amount)
        {
            var existingClient = _context.Clients.FirstOrDefault(c => c.ClientId == clientId);
            if (existingClient == null)
                throw new ArgumentException("Client not found");

            existingClient.Balance += amount;
            _context.SaveChanges();
        }

        public void ClientWithdraw(int clientId , decimal amount)
        {
            var existingClient = _context.Clients.FirstOrDefault(c => c.ClientId == clientId);

            if (existingClient == null)
                throw new ArgumentException("Client not found");

            if (amount <= 0)
                throw new ArgumentException("Invalid withdraw amount.");

            if (existingClient.Balance < amount)
                throw new InvalidOperationException("Insufficient balance.");

            existingClient.Balance -= amount;
            _context.SaveChanges();


        }
        
    }
}
