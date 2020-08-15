using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using BalanceManagerApi.Controllers;
using BalanceManagerApi.ViewModels;
using System;
using Xunit;
using Balances;

namespace BalanceManager.UnitTests.Tests
{
    public class DepositTests : TestsBase
    {
        #region Private Fields
        private Mock<ILogger<DepositController>> _logger;
        private DepositController _depositController;
        #endregion

        #region Constructors
        public DepositTests() : base()
        {
            _logger = new Mock<ILogger<DepositController>>();
            _depositController = new DepositController(
                _casinoBalanceManager,
                _gameBalanceManager,
                _logger.Object);
        }
        #endregion

        #region Tests
        [InlineData(-500)]
        [InlineData(1000)]
        [InlineData(2000)]
        [InlineData(1000000)]
        [Theory]
        public void DepositTests_Deposit_Test(decimal amount)
        {
            string transactionId = Guid.NewGuid().ToString();

            ErrorCode chechTransactionInGameBalanceManager = _gameBalanceManager.CheckTransaction(transactionId);
            Assert.Equal(ErrorCode.TransactionNotFound, chechTransactionInGameBalanceManager);

            ErrorCode chechTransactionInCasinoBalanceManager = _casinoBalanceManager.CheckTransaction(transactionId);
            Assert.Equal(ErrorCode.TransactionNotFound, chechTransactionInCasinoBalanceManager);

            decimal gameBalanceStart = _gameBalanceManager.GetBalance();
            decimal casinoBalanceStart = _casinoBalanceManager.GetBalance();

            Assert.True(gameBalanceStart > 0);
            Assert.True(casinoBalanceStart > 0);

            var depositFundsResult = _depositController.DepositFunds(amount, transactionId);

            decimal gameBalanceEnd = _gameBalanceManager.GetBalance();
            decimal casinoBalanceEnd = _casinoBalanceManager.GetBalance();

            if (amount > gameBalanceStart)
            {
                Assert.Equal(gameBalanceStart, gameBalanceEnd);
                Assert.Equal(casinoBalanceStart, casinoBalanceEnd);

                BadRequestObjectResult depositFundsErrorResult = Assert.IsType<BadRequestObjectResult>(depositFundsResult);
                var errorResultValue = Assert.IsType<ErrorModel>((ErrorModel)depositFundsErrorResult.Value);

                Assert.Equal(ErrorCode.NotEnoughtBalance.ToString(), errorResultValue.ErrorCode);
            }
            else if (amount <= 0)
            {
                Assert.Equal(gameBalanceStart, gameBalanceEnd);
                Assert.Equal(casinoBalanceStart, casinoBalanceEnd);

                BadRequestObjectResult depositFundsErrorResult = Assert.IsType<BadRequestObjectResult>(depositFundsResult);
                var errorResultValue = Assert.IsType<ErrorModel>((ErrorModel)depositFundsErrorResult.Value);

                Assert.Equal("NegativeOrZeroValue", errorResultValue.ErrorCode);
            }
            else
            {
                Assert.Equal(gameBalanceStart - amount, gameBalanceEnd);
                Assert.Equal(casinoBalanceStart + amount, casinoBalanceEnd);

                Assert.IsType<OkResult>(depositFundsResult);
            }
        }
        #endregion
    }
}
