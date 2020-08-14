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
        #region Private fields
        private readonly IBalanceManager _balanceManager;
        private readonly GameBalanceManager _gameBalanceManager;
        private readonly ILogger<WithdrawController> _logger;
        #endregion

        #region Constructors
        public WithdrawController(CasinoBalanceManager casinoBalanceManager, 
            ILogger<WithdrawController> logger)
        {
            _balanceManager = casinoBalanceManager;

            _logger = logger;
        }
        #endregion

        #region Api Methods
        [HttpPost]
        [Route("{transactionId}/{amount:decimal}")]
        public ActionResult<ErrorModel> DecreaseBalance(decimal amount, string transactionId)
        {
            _logger.LogInformation($"Withdraw/{transactionId}/{amount} was called");

            ErrorCode decreaseBalanceResult = _balanceManager.DecreaseBalance(amount, transactionId);
            ErrorModel errorResult = new ErrorModel();

            try
            {
                if (decreaseBalanceResult == ErrorCode.Success)
                {
                    return Ok();
                }
                else
                {
                    errorResult.ErrorCode = decreaseBalanceResult.ToString();
                    errorResult.ErrorDescription = decreaseBalanceResult.GetErrorDescription();

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
        }
        #endregion
    }
}
