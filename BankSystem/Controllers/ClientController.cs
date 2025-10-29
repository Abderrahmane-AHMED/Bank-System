

using Domain;
using Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ILogger<ClientController> _logger;
        public ClientController(IClientService clientService, ILogger<ClientController> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            var clients = _clientService.GetAllClients();
            return View(clients);
        }



        [HttpGet]
        public IActionResult Update(int clientId)
        {
            var client = _clientService.GetClientById(clientId);
            if (client == null)
            {
                TempData["ErrorMessage"] = "Client not found.";
                return RedirectToAction("ErrorPage");
            }
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(TbClient client)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(client);

                _clientService.Update(client);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating client {client.ClientId}");
                TempData["ErrorMessage"] = "An error occurred while updating the client.";
                return RedirectToAction("ErrorPage");
            }
        }

        #region   Add 

        [HttpGet]
        public IActionResult Add()
        {
            return View(new TbClient());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TbClient client)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(client);
                }
                ;

                var existingClient = _clientService.GetAllClients()
          .FirstOrDefault(c => c.AccountNumber == client.AccountNumber && c.ClientId != client.ClientId);

                if (existingClient != null)
                {
                    ModelState.AddModelError("AccountNumber", "The account number already exists, please enter another number.");
                    return View("Update", client);
                }
                _clientService.Add(client);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while saving client. ID: {client?.ClientId}");
                TempData["ErrorMessage"] = "An error occurred while saving the client.";
                return RedirectToAction("ErrorPage");
            }
        }

        #endregion

        #region  Delete

        public IActionResult ClientDelete(int clientId)
        {
            try
            {
                _clientService.ClientDelete(clientId);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting client. ID: {clientId}");
                TempData["ErrorMessage"] = "An error occurred while deleting the client.";
                return RedirectToAction("ErrorPage");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int clientId)
        {
            _clientService.ClientDelete(clientId);
            return RedirectToAction("DeleteConfirmation", new { clientId });
        }


        #endregion

        #region  Look Up For Update

        [HttpGet]
        public IActionResult LookupForUpdate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LookupForUpdate(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                ModelState.AddModelError("", "Please enter account number.");
                return View();
            }

            var client = _clientService.FindByAccountNumber(accountNumber);
            if (client == null)
            {
                ModelState.AddModelError("", "Client not found.");
                return View();
            }

            return RedirectToAction("Update", new { clientId = client.ClientId });
        }


        #endregion


        #region  Look Up For Delete

        [HttpGet]
        public IActionResult LookupForDelete()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LookupForDelete(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                ModelState.AddModelError("", "Please enter account number.");
                return View();
            }

            var client = _clientService.FindByAccountNumber(accountNumber);
            if (client == null)
            {
                ModelState.AddModelError("", "Client not found.");
                return View();
            }


            return RedirectToAction("Delete", new { clientId = client.ClientId });
        }

        #endregion


        public IActionResult ErrorPage()
        {
            return View();
        }


    }
}
