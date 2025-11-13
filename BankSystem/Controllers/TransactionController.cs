using Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ILogger<TransactionController> _logger;
        public TransactionController(IClientService clientService, ILogger<TransactionController> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }
        public IActionResult TransactionMenueScreen()
        {
            return View();
        }

        #region   Deposit
        [HttpGet]
        public IActionResult Deposit(int clientId)
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

        [HttpGet]
        public IActionResult ErrorPage()
        {
            
            ViewBag.ErrorMessage = TempData["ErrorMessage"]?.ToString();
            return View();
        }

    }
}
