using Balances;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetCoreBalanceManager.Tests.Tests;
using NetCoreBalanceManagerApi.Controllers;
using System;

namespace NetCoreBalanceManager.Tests
{
    [TestClass]
    public class BalanceTests : TestsBase
    {
        private Mock<ILogger<BalanceController>> _logger;
        private BalanceController _balanceController;

        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();

            _logger = new Mock<ILogger<BalanceController>>();
            _balanceController = new BalanceController(
                _casinoBalanceManager,
                _gameBalanceManager,
                _logger.Object);
        }

        [TestMethod]
        public void Test_1_StartBalance()
        {
            var getBalanceResult = _balanceController.GetBalance();
            decimal casinoBalanceValue = _casinoBalanceManager.GetBalance();
            decimal gamebalanceValue = _gameBalanceManager.GetBalance();

            Assert.IsInstanceOfType(getBalanceResult, typeof(ActionResult<decimal>));
            
            //Assert.AreEqual(casinoBalanceValue, balanceValue);
            Assert.AreEqual(10000, casinoBalanceValue);
            Assert.AreEqual(10000, gamebalanceValue);
        }

        [TestMethod]
        public void Test_2_CasinoBalanceDeposit()
        {
            string transactionId = Guid.NewGuid().ToString();
            decimal casinoExpectedBalance = _casinoBalanceManager.GetBalance();
            _casinoBalanceManager.IncreaseBalance(100, transactionId);
            casinoExpectedBalance += 100;
            decimal casinoActualBalance = _casinoBalanceManager.GetBalance();

            Assert.AreEqual(casinoExpectedBalance, casinoActualBalance);
        }

        [TestMethod]
        public void Test_3_CasinoBalanceWithdraw()
        {
            string transactionId = Guid.NewGuid().ToString();
            decimal casinoExpectedBalance = _casinoBalanceManager.GetBalance();
            _casinoBalanceManager.DecreaseBalance(100, transactionId);
            casinoExpectedBalance -= 100;
            decimal casinoActualBalance = _casinoBalanceManager.GetBalance();

            Assert.AreEqual(casinoExpectedBalance, casinoActualBalance);
        }


        [TestMethod]
        public void Test_4_GameBalanceDeposit()
        {
            string transactionId = Guid.NewGuid().ToString();
            decimal gameExpectedBalance = _gameBalanceManager.GetBalance();
            _gameBalanceManager.IncreaseBalance(100, transactionId);
            gameExpectedBalance += 100;
            decimal gameActualBalance = _gameBalanceManager.GetBalance();

            Assert.AreEqual(gameExpectedBalance, gameActualBalance);
        }

        [TestMethod]
        public void Test_5_GameBalanceWithdraw()
        {
            string transactionId = Guid.NewGuid().ToString();
            decimal gameExpectedBalance = _gameBalanceManager.GetBalance();
            _gameBalanceManager.DecreaseBalance(100, transactionId);
            gameExpectedBalance -= 100;
            decimal gameActualBalance = _gameBalanceManager.GetBalance();

            Assert.AreEqual(gameExpectedBalance, gameActualBalance);
        }
    }
}
