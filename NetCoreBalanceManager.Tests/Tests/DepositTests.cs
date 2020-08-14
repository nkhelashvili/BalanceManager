using Balances;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetCoreBalanceManagerApi.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreBalanceManager.Tests.Tests
{
    [TestClass]
    public class DepositTests : TestsBase
    {
        private Mock<ILogger<DepositController>> _logger;
        private DepositController _depositController;

        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();

            _logger = new Mock<ILogger<DepositController>>();
            _depositController = new DepositController(
                _casinoBalanceManager,
                _gameBalanceManager,
                _logger.Object);
        }

    }
}
