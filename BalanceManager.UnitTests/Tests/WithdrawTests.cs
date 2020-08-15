using Balances;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using BalanceManagerApi.Controllers;
using BalanceManagerApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BalanceManager.UnitTests.Tests
{
    public class WithdrawTests : TestsBase
    {
        #region Private Fields
        private Mock<ILogger<WithdrawController>> _logger;
        private WithdrawController _withdrawController;
        #endregion

        #region Constructors
        public WithdrawTests() : base()
        {
            _logger = new Mock<ILogger<WithdrawController>>();
            _withdrawController = new WithdrawController(
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
        public void WithdrawTests_Withdraw_Test(decimal amount)
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

            var depositFundsResult = _withdrawController.Withdraw(amount, transactionId);

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
                Assert.Equal(gameBalanceStart + amount, gameBalanceEnd);
                Assert.Equal(casinoBalanceStart - amount, casinoBalanceEnd);

                Assert.IsType<OkResult>(depositFundsResult);
            }
        }
        #endregion
    }
}
