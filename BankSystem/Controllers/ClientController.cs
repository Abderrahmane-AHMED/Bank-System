

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

        #region  List

        public IActionResult List()
        {
            var clients = _clientService.GetAllClients();
            return View(clients);
        }

        #endregion

        #region Find Client

        [HttpGet]
        public IActionResult FindClient()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FindClient(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                ModelState.AddModelError("", "Please enter an account number.");
                return View();
            }

            var client = _clientService.FindByAccountNumber(accountNumber);
            if (client == null)
            {
                ModelState.AddModelError("", "Client not found.");
                return View();
            }

           
            return View(client);
        }

        #endregion


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

        #region Update


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
                return RedirectToAction("UpdateConfirmation", new { clientId = client.ClientId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating client {client.ClientId}");
                TempData["ErrorMessage"] = "An error occurred while updating the client.";
                return RedirectToAction("ErrorPage");
            }
        }
        [HttpGet]
        public IActionResult UpdateConfirmation(int clientId)
        {
            var client = _clientService.GetClientById(clientId);
            return View(client);
        }

        #endregion


        #region Delete

        [HttpGet]
        public IActionResult Delete(int clientId)
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
        public IActionResult DeleteConfirmed(int clientId)
        {
            try
            {
                _clientService.ClientDelete(clientId);
                return RedirectToAction("DeleteConfirmation", new { clientId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting client. ID: {clientId}");
                TempData["ErrorMessage"] = "An error occurred while deleting the client.";
                return RedirectToAction("ErrorPage");
            }
        }

        [HttpGet]
        public IActionResult DeleteConfirmation(int clientId)
        {
            var client = _clientService.GetClientById(clientId);
            return View(client);
        }

        #endregion

        #region   Deposit
        [HttpGet]
        public IActionResult Deposit(int clientId)
        {
            var client = _clientService.GetClientById(clientId);
            if(client == null)
            {
                TempData["ErrorMessage"] = "Client not found.";
                return RedirectToAction("ErrorPage");
            }
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deposit(int clientId, decimal depositAmount)
        {
            try
            {
                if (depositAmount <= 0)
                {
                    ModelState.AddModelError("", "Please enter a valid deposit amount greater than 0.");
                    var existing = _clientService.GetClientById(clientId);
                    return View(existing);
                }

                _clientService.ClientDeposit(clientId, depositAmount);

                return RedirectToAction("DepositConfirmation", new { clientId = clientId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Depositing client {clientId}");
                TempData["ErrorMessage"] = "An error occurred while depositing the client.";
                return RedirectToAction("ErrorPage");
            }
        }


        [HttpGet]
        public IActionResult DepositConfirmation(int clientId)
        {
            var client = _clientService.GetClientById(clientId);
            return View(client);
        }
        #endregion

        #region  Client Withdraw
        [HttpGet]
        public IActionResult Withdraw(int clientId)
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
        public IActionResult Withdraw(int clientId, decimal withdrawAmount)
        {
            try
            {
                if (withdrawAmount <= 0)
                {
                    ModelState.AddModelError("", "Please enter a valid withdraw amount greater than 0.");
                    var client = _clientService.GetClientById(clientId);
                    return View(client);
                }

                _clientService.ClientWithdraw(clientId, withdrawAmount);

                return RedirectToAction("WithdrawConfirmation", new { clientId = clientId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error withdrawing client {clientId}");
                TempData["ErrorMessage"] = "An error occurred while withdrawing from the client account.";
                return RedirectToAction("ErrorPage");
            }
        }

        [HttpGet]
        public IActionResult WithdrawConfirmation(int clientId)
        {
            var client = _clientService.GetClientById(clientId);
            return View(client);
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

        #region  Look Up For Deposit

        [HttpGet]
        public IActionResult LookupForDeposit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LookupForDeposit(string accountNumber)
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

            return RedirectToAction("Deposit", new { clientId = client.ClientId });
        }


        #endregion

        #region  Look Up For Withdraw

        [HttpGet]
        public IActionResult LookupForWithdraw()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LookupForWithdraw(string accountNumber)
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

            return RedirectToAction("Withdraw", new { clientId = client.ClientId });
        }


        #endregion
        public IActionResult ErrorPage()
        {
            return View();
        }


    }
}
