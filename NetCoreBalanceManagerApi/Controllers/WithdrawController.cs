using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Balances;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreBalanceManagerApi.Constants;
using NetCoreBalanceManagerApi.Extensions;
using NetCoreBalanceManagerApi.ViewModels;

namespace NetCoreBalanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WithdrawController : ControllerBase
    {
        #region Private Fields
        private readonly CasinoBalanceManager _casinoBalanceManager;
        private readonly GameBalanceManager _gameBalanceManager;
        private readonly ILogger<WithdrawController> _logger;

        #endregion

        #region Constructors
        public WithdrawController(CasinoBalanceManager casinoBalanceManager,
                 GameBalanceManager gameBalanceManager,
                 ILogger<WithdrawController> logger)
        {
            _gameBalanceManager = gameBalanceManager;
            _casinoBalanceManager = casinoBalanceManager;
            _logger = logger;
        }
        #endregion

        #region Api Methods
        [HttpPost]
        [Route("{transactionId}/{amount:decimal}")]
        public ActionResult<ErrorModel> Withdraw(decimal amount, string transactionId)
        {
            _logger.LogInformation($"Withdraw/{transactionId}/{amount} was called");

            return null;
        }

        #endregion
    }
}
