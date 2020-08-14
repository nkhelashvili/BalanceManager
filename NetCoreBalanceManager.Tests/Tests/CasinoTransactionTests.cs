using Balances;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreBalanceManager.Tests.Tests
{
    [TestClass]
    public class CasinoTransactionTests : TestsBase
    {
        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();

            _balanceManager = _casinoBalanceManager;
        }

        [TestMethod]
        public void CasinoTransactionTests_1_Check_Casino_IncreaseBalance_PipeLine()
        {
            string transactionId = Guid.NewGuid().ToString();

            ErrorCode transactionTestBeforeIncrease = _balanceManager.CheckTransaction(transactionId);
            Assert.AreEqual(ErrorCode.TransactionNotFound, transactionTestBeforeIncrease);

            decimal startBalance = _balanceManager.GetBalance();
            ErrorCode increaseTestResult = _balanceManager.IncreaseBalance(500, transactionId);
            Assert.AreEqual(ErrorCode.Success, increaseTestResult);
            decimal endBalance = _balanceManager.GetBalance();
            Assert.AreEqual(endBalance, startBalance + 500);

            ErrorCode transactionDuplicationTest = _balanceManager.IncreaseBalance(200, transactionId);
            Assert.AreEqual(ErrorCode.DuplicateTransactionId, transactionDuplicationTest);

            ErrorCode addedTransactionTest = _balanceManager.CheckTransaction(transactionId);
            Assert.AreEqual(ErrorCode.Success, addedTransactionTest);

            ErrorCode rollbackResultTest = _balanceManager.Rollback(transactionId);
            Assert.AreEqual(ErrorCode.Success, rollbackResultTest);

            ErrorCode transactionTestAfterRollback = _balanceManager.CheckTransaction(transactionId);
            Assert.AreEqual(ErrorCode.TransactionRollbacked, transactionTestAfterRollback);

            ErrorCode doubleRollbackTest = _balanceManager.Rollback(transactionId);
            Assert.AreEqual(ErrorCode.TransactionAlreadyMarkedAsRollback, doubleRollbackTest);

            ErrorCode transactionCheckAfterSecondRollbackTry = _balanceManager.CheckTransaction(transactionId);
            Assert.AreEqual(ErrorCode.TransactionRollbacked, transactionCheckAfterSecondRollbackTry);

            endBalance = _balanceManager.GetBalance();
            Assert.AreEqual(startBalance, endBalance);
            /* 
             * CasinoBalanceManager-ის ტრანზაქციის როლბექი არ მუშაობს სწორად
             * გაზრდის როლბექი შემცირების მაგივრად ზრდის თანხას
             */
        }

        [TestMethod]
        public void CasinoTransactionTests_2_Check_Casino_DecreaseBalance_PipeLine()
        {
            string transactionId = Guid.NewGuid().ToString();

            ErrorCode transactionTestBeforeDecrease = _balanceManager.CheckTransaction(transactionId);
            Assert.AreEqual(ErrorCode.TransactionNotFound, transactionTestBeforeDecrease);

            decimal startBalance = _balanceManager.GetBalance();
            ErrorCode increaseTestResult = _balanceManager.DecreaseBalance(500, transactionId);
            Assert.AreEqual(ErrorCode.Success, increaseTestResult);
            decimal endBalance = _balanceManager.GetBalance();
            Assert.AreEqual(endBalance, startBalance - 500);

            ErrorCode transactionDuplicationTest = _balanceManager.DecreaseBalance(200, transactionId);
            Assert.AreEqual(ErrorCode.DuplicateTransactionId, transactionDuplicationTest);

            ErrorCode addedTransactionTest = _balanceManager.CheckTransaction(transactionId);
            Assert.AreEqual(ErrorCode.Success, addedTransactionTest);

            ErrorCode rollbackResultTest = _balanceManager.Rollback(transactionId);
            Assert.AreEqual(ErrorCode.Success, rollbackResultTest);

            ErrorCode transactionTestAfterRollback = _balanceManager.CheckTransaction(transactionId);
            Assert.AreEqual(ErrorCode.TransactionRollbacked, transactionTestAfterRollback);

            ErrorCode doubleRollbackTest = _balanceManager.Rollback(transactionId);
            Assert.AreEqual(ErrorCode.TransactionAlreadyMarkedAsRollback, doubleRollbackTest);

            ErrorCode transactionCheckAfterSecondRollbackTry = _balanceManager.CheckTransaction(transactionId);
            Assert.AreEqual(ErrorCode.TransactionRollbacked, transactionCheckAfterSecondRollbackTry);

            endBalance = _balanceManager.GetBalance();
            Assert.AreEqual(startBalance, endBalance);
            /* 
             * CasinoBalanceManager-ის ტრანზაქციის როლბექი არ მუშაობს სწორად
             * შემცირების როლბექი გაზრდის მაგივრად მაგივრად ამცირებს თანხას
             */
        }
    }
}
