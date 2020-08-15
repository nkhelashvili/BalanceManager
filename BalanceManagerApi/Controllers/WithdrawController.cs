using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Balances;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BalanceManagerApi.Constants;
using BalanceManagerApi.Extensions;
using BalanceManagerApi.ViewModels;

namespace BalanceManagerApi.Controllers
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
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        [Route("{transactionId}/{amount:decimal}")]
        public IActionResult Withdraw(decimal amount, string transactionId)
        {
            _logger.LogInformation($"Withdraw/{transactionId}/{amount} was called");

            ErrorModel errorResult = new ErrorModel();

            if (amount <= 0)
            {
                errorResult.ErrorCode = "NegativeOrZeroValue";
                errorResult.ErrorDescription = "უარყოფითი ან ნულოვანი მნიშვნელობის გადაცემა არაა დაშვებული";

                return BadRequest(errorResult);
            }

            ErrorCode casinoDecreaseBalanceResult = _casinoBalanceManager.DecreaseBalance(amount, transactionId);

            try
            {
                if (casinoDecreaseBalanceResult != ErrorCode.Success)
                {
                    errorResult.ErrorCode = casinoDecreaseBalanceResult.ToString();
                    errorResult.ErrorDescription = casinoDecreaseBalanceResult.GetErrorDescription() + $" (CasinoBalance)";

                    return BadRequest(errorResult);
                }
            }
            catch (Exception ex)
            {
                errorResult.ErrorCode = ErrorCode.UnknownError.ToString();
                errorResult.ErrorDescription = ErrorCode.UnknownError.GetErrorDescription();
                _logger.LogError($"Error executing Withdraw/{transactionId}/{amount}: {ex.Message}");
                return BadRequest(ErrorCode.UnknownError);
            }

            ErrorCode gameIncreaseBalanceResult = _gameBalanceManager.IncreaseBalance(amount, transactionId);

            if (gameIncreaseBalanceResult != ErrorCode.Success)
            {
                gameIncreaseBalanceResult = _casinoBalanceManager.Rollback(transactionId);

                if (gameIncreaseBalanceResult == ErrorCode.Success)
                {
                    gameIncreaseBalanceResult = ErrorCode.TransactionRollbacked;
                }
            }

            try
            {
                if (casinoDecreaseBalanceResult != ErrorCode.Success)
                {
                    errorResult.ErrorCode = gameIncreaseBalanceResult.ToString();
                    if (gameIncreaseBalanceResult == ErrorCode.TransactionRollbacked)
                    {
                        errorResult.ErrorDescription = gameIncreaseBalanceResult.GetErrorDescription() + $" (GameBalance)";
                    }
                    else
                    {
                        errorResult.ErrorDescription = gameIncreaseBalanceResult.GetErrorDescription() + $" (CasinoBalance)";
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
                _logger.LogError($"Error executing Withdraw/{transactionId}/{amount}: {ex.Message}");
                return BadRequest(ErrorCode.UnknownError);
            }
        }

        #endregion
    }
}
