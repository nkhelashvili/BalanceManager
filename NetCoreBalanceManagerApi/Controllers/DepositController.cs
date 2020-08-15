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

        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        [Route("{transactionId}/{amount:decimal}")]
        public IActionResult DepositFunds(decimal amount, string transactionId)
        {
            _logger.LogInformation($"Deposit/{transactionId}/{amount} was called");
            ErrorModel errorResult = new ErrorModel();

            if (amount <= 0)
            {
                errorResult.ErrorCode = "NegativeOrZeroValue";
                errorResult.ErrorDescription = "უარყოფითი ან ნულოვანი მნიშვნელობის გადაცემა არაა დაშვებული";

                return BadRequest(errorResult);
            }

            ErrorCode gameDecreaseBalanceResult = _gameBalanceManager.DecreaseBalance(amount, transactionId);

            try
            {
                if (gameDecreaseBalanceResult != ErrorCode.Success)
                {
                    errorResult.ErrorCode = gameDecreaseBalanceResult.ToString();
                    errorResult.ErrorDescription = gameDecreaseBalanceResult.GetErrorDescription() + $" (GameBalance)";

                    return BadRequest(errorResult);
                }
            }
            catch (Exception ex)
            {
                errorResult.ErrorCode = ErrorCode.UnknownError.ToString();
                errorResult.ErrorDescription = ErrorCode.UnknownError.GetErrorDescription();
                _logger.LogError($"Error executing Deposit/{transactionId}/{amount}: {ex.Message}");
                return BadRequest(ErrorCode.UnknownError);
            }

            ErrorCode casinoIncreaseBalanceResult = _casinoBalanceManager.IncreaseBalance(amount, transactionId);

            if (casinoIncreaseBalanceResult != ErrorCode.Success)
            {
                casinoIncreaseBalanceResult = _gameBalanceManager.Rollback(transactionId);

                if (casinoIncreaseBalanceResult == ErrorCode.Success)
                {
                    casinoIncreaseBalanceResult = ErrorCode.TransactionRollbacked;
                }
            }

            try
            {
                if (gameDecreaseBalanceResult != ErrorCode.Success)
                {
                    errorResult.ErrorCode = casinoIncreaseBalanceResult.ToString();
                    if (casinoIncreaseBalanceResult == ErrorCode.TransactionRollbacked)
                    {
                        errorResult.ErrorDescription = casinoIncreaseBalanceResult.GetErrorDescription() + $" (GameBalance)";
                    }
                    else
                    {
                        errorResult.ErrorDescription = casinoIncreaseBalanceResult.GetErrorDescription() + $" (CasinoBalance)";
                    }

                    return BadRequest(errorResult);
                }
                else
                {
                    return Ok();
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
