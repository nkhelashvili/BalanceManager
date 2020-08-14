using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Balances;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreBalanceManagerApi.Extensions;
using NetCoreBalanceManagerApi.ViewModels;

namespace NetCoreBalanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        #region Private Fields
        private readonly CasinoBalanceManager _casinoBalanceManager;
        private readonly GameBalanceManager _gameBalanceManager;
        private readonly ILogger<DepositController> _logger;

        #endregion

        #region Constructors
        public DepositController(CasinoBalanceManager casinoBalanceManager, 
            GameBalanceManager gameBalanceManager,
            ILogger<DepositController> logger)
        {
            _gameBalanceManager = gameBalanceManager;
            _casinoBalanceManager = casinoBalanceManager;
            _logger = logger;
        }
        #endregion

        #region Api Methods
        [HttpPost]
        [Route("{transactionId}/{amount:decimal}")]
        public ActionResult<ErrorModel> DepositFunds(decimal amount, string transactionId)
        {
            _logger.LogInformation($"Deposit/{transactionId}/{amount} was called");

            ErrorCode depositFundsResult = _casinoBalanceManager.IncreaseBalance(amount, transactionId);
            ErrorModel errorResult = new ErrorModel();

            try
            {
                if (depositFundsResult == ErrorCode.Success)
                {
                    return Ok();
                }
                else
                {
                    errorResult.ErrorCode = depositFundsResult.ToString();
                    errorResult.ErrorDescription = depositFundsResult.GetErrorDescription();

                    return BadRequest(depositFundsResult);
                }
            }
            catch (Exception ex)
            {
                errorResult.ErrorCode = ErrorCode.UnknownError.ToString();
                errorResult.ErrorDescription = ErrorCode.UnknownError.GetErrorDescription();
                _logger.LogError($"Error executing Deposit/{transactionId}/{amount}: {ex.Message}");
                return BadRequest(ErrorCode.UnknownError);
            }
        }

        #endregion
    }
}
