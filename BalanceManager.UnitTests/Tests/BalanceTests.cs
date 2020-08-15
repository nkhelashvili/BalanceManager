using BalanceManagerApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace BalanceManager.UnitTests.Tests
{
    public class BalanceTests : TestsBase
    {
        #region Private Fields
        private Mock<ILogger<BalanceController>> _logger;
        private BalanceController _balanceController;
        #endregion

        #region Contructors
        public BalanceTests() : base()
        {
            _logger = new Mock<ILogger<BalanceController>>();
            _balanceController = new BalanceController(
                _casinoBalanceManager,
                _gameBalanceManager,
                _logger.Object);
        }
        #endregion

        #region Tests
        [Fact]
        public void Test_1_StartBalance()
        {
            var getBalanceActionResult = _balanceController.GetBalance();
            decimal casinoBalanceValue = _casinoBalanceManager.GetBalance();

            ActionResult<decimal> getBalanceResut = Assert.IsType<ActionResult<decimal>>(getBalanceActionResult);
            Assert.IsType<OkObjectResult>(getBalanceResut.Result);

            var balanceValue  = (decimal)((ObjectResult)getBalanceResut.Result).Value;
            Assert.Equal(casinoBalanceValue, balanceValue);
        }

        [Fact]
        public void Test_2_CasinoBalanceDeposit()
        {
            string transactionId = Guid.NewGuid().ToString();
            decimal casinoExpectedBalance = _casinoBalanceManager.GetBalance();
            _casinoBalanceManager.IncreaseBalance(100, transactionId);
            casinoExpectedBalance += 100;
            decimal casinoActualBalance = _casinoBalanceManager.GetBalance();

            Assert.Equal(casinoExpectedBalance, casinoActualBalance);
        }

        [Fact]
        public void Test_3_CasinoBalanceWithdraw()
        {
            string transactionId = Guid.NewGuid().ToString();
            decimal casinoExpectedBalance = _casinoBalanceManager.GetBalance();
            _casinoBalanceManager.DecreaseBalance(100, transactionId);
            casinoExpectedBalance -= 100;
            decimal casinoActualBalance = _casinoBalanceManager.GetBalance();

            Assert.Equal(casinoExpectedBalance, casinoActualBalance);
        }


        [Fact]
        public void Test_4_GameBalanceDeposit()
        {
            string transactionId = Guid.NewGuid().ToString();
            decimal gameExpectedBalance = _gameBalanceManager.GetBalance();
            _gameBalanceManager.IncreaseBalance(100, transactionId);
            gameExpectedBalance += 100;
            decimal gameActualBalance = _gameBalanceManager.GetBalance();

            Assert.Equal(gameExpectedBalance, gameActualBalance);
        }

        [Fact]
        public void Test_5_GameBalanceWithdraw()
        {
            string transactionId = Guid.NewGuid().ToString();
            decimal gameExpectedBalance = _gameBalanceManager.GetBalance();
            _gameBalanceManager.DecreaseBalance(100, transactionId);
            gameExpectedBalance -= 100;
            decimal gameActualBalance = _gameBalanceManager.GetBalance();

            Assert.Equal(gameExpectedBalance, gameActualBalance);
        }
        #endregion
    }
}
