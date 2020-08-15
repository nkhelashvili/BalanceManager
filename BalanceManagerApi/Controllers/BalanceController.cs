using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Balances;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BalanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {

        #region Private fields
        private readonly CasinoBalanceManager _casinoBalanceManager;
        private readonly GameBalanceManager _gameBalanceManager;
        private readonly ILogger<BalanceController> _logger;
        #endregion

        #region Constructors
        public BalanceController(CasinoBalanceManager casinoBalanceManager,
                        GameBalanceManager gameBalanceManager,
                        ILogger<BalanceController> logger)
        {
            _gameBalanceManager = gameBalanceManager;
            _casinoBalanceManager = casinoBalanceManager;
            _logger = logger;
        }
        #endregion

        #region Api Methods
        [HttpGet]
        public  ActionResult<decimal> GetBalance()
        {
            _logger.LogInformation($"Balance get was called");
            _logger.LogInformation($"Casino balance: {_casinoBalanceManager.GetBalance()}");
            _logger.LogInformation($"Game balance: {_gameBalanceManager.GetBalance()}");

            return Ok(_casinoBalanceManager.GetBalance());
        }
        #endregion
    }
}
