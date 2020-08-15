using Balances;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BalanceManager.UnitTests.Tests
{
    public class CasinoTransactionTests : TestsBase
    {
        #region Constructors
        public CasinoTransactionTests() : base()
        {
            _balanceManager = _casinoBalanceManager;
        }
        #endregion

        #region Tests
        [Fact]
        public void CasinoTransactionTests_1_Check_Casino_IncreaseBalance_PipeLine()
        {
            string transactionId = Guid.NewGuid().ToString();

            ErrorCode transactionTestBeforeIncrease = _balanceManager.CheckTransaction(transactionId);
            Assert.Equal(ErrorCode.TransactionNotFound, transactionTestBeforeIncrease);

            decimal startBalance = _balanceManager.GetBalance();
            ErrorCode increaseTestResult = _balanceManager.IncreaseBalance(500, transactionId);
            Assert.Equal(ErrorCode.Success, increaseTestResult);
            decimal endBalance = _balanceManager.GetBalance();
            Assert.Equal(endBalance, startBalance + 500);

            ErrorCode transactionDuplicationTest = _balanceManager.IncreaseBalance(200, transactionId);
            Assert.Equal(ErrorCode.DuplicateTransactionId, transactionDuplicationTest);

            ErrorCode addedTransactionTest = _balanceManager.CheckTransaction(transactionId);
            Assert.Equal(ErrorCode.Success, addedTransactionTest);

            ErrorCode rollbackResultTest = _balanceManager.Rollback(transactionId);
            Assert.Equal(ErrorCode.Success, rollbackResultTest);

            ErrorCode transactionTestAfterRollback = _balanceManager.CheckTransaction(transactionId);
            Assert.Equal(ErrorCode.TransactionRollbacked, transactionTestAfterRollback);

            ErrorCode doubleRollbackTest = _balanceManager.Rollback(transactionId);
            Assert.Equal(ErrorCode.TransactionAlreadyMarkedAsRollback, doubleRollbackTest);

            ErrorCode transactionCheckAfterSecondRollbackTry = _balanceManager.CheckTransaction(transactionId);
            Assert.Equal(ErrorCode.TransactionRollbacked, transactionCheckAfterSecondRollbackTry);

            endBalance = _balanceManager.GetBalance();
            Assert.Equal(startBalance, endBalance);
            /* 
             * CasinoBalanceManager-ის ტრანზაქციის როლბექი არ მუშაობს სწორად
             * გაზრდის როლბექი შემცირების მაგივრად ზრდის თანხას
             */
        }

        [Fact]
        public void CasinoTransactionTests_2_Check_Casino_DecreaseBalance_PipeLine()
        {
            string transactionId = Guid.NewGuid().ToString();

            ErrorCode transactionTestBeforeDecrease = _balanceManager.CheckTransaction(transactionId);
            Assert.Equal(ErrorCode.TransactionNotFound, transactionTestBeforeDecrease);

            decimal startBalance = _balanceManager.GetBalance();
            ErrorCode increaseTestResult = _balanceManager.DecreaseBalance(500, transactionId);
            Assert.Equal(ErrorCode.Success, increaseTestResult);
            decimal endBalance = _balanceManager.GetBalance();
            Assert.Equal(endBalance, startBalance - 500);

            ErrorCode transactionDuplicationTest = _balanceManager.DecreaseBalance(200, transactionId);
            Assert.Equal(ErrorCode.DuplicateTransactionId, transactionDuplicationTest);

            ErrorCode addedTransactionTest = _balanceManager.CheckTransaction(transactionId);
            Assert.Equal(ErrorCode.Success, addedTransactionTest);

            ErrorCode rollbackResultTest = _balanceManager.Rollback(transactionId);
            Assert.Equal(ErrorCode.Success, rollbackResultTest);

            ErrorCode transactionTestAfterRollback = _balanceManager.CheckTransaction(transactionId);
            Assert.Equal(ErrorCode.TransactionRollbacked, transactionTestAfterRollback);

            ErrorCode doubleRollbackTest = _balanceManager.Rollback(transactionId);
            Assert.Equal(ErrorCode.TransactionAlreadyMarkedAsRollback, doubleRollbackTest);

            ErrorCode transactionCheckAfterSecondRollbackTry = _balanceManager.CheckTransaction(transactionId);
            Assert.Equal(ErrorCode.TransactionRollbacked, transactionCheckAfterSecondRollbackTry);

            endBalance = _balanceManager.GetBalance();
            Assert.Equal(startBalance, endBalance);
            /* 
             * CasinoBalanceManager-ის ტრანზაქციის როლბექი არ მუშაობს სწორად
             * შემცირების როლბექი გაზრდის მაგივრად მაგივრად ამცირებს თანხას
             */
        }

        [Fact]
        public void CasinoTransactionTests_3_TryDecreaseWithMoreThenBalanceAmount()
        {
            string transactionId = Guid.NewGuid().ToString();

            ErrorCode transactionTestBeforeDecrease = _balanceManager.CheckTransaction(transactionId);
            Assert.Equal(ErrorCode.TransactionNotFound, transactionTestBeforeDecrease);

            decimal startBalance = _balanceManager.GetBalance();
            ErrorCode decreaseTestResult = _balanceManager.DecreaseBalance(startBalance + 1, transactionId);
            Assert.Equal(ErrorCode.NotEnoughtBalance, decreaseTestResult);
            decimal endBalance = _balanceManager.GetBalance();
            Assert.Equal(startBalance, endBalance);
        }
        #endregion
    }
}
