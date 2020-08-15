using Microsoft.Extensions.Logging;
using Moq;
using NetCoreBalanceManagerApi.Controllers;

namespace NetCoreBalanceManager.Tests.Tests
{
    public class DepositTests : TestsBase
    {
        private Mock<ILogger<DepositController>> _logger;
        private DepositController _depositController;

        public DepositTests() : base()
        {
            _logger = new Mock<ILogger<DepositController>>();
            _depositController = new DepositController(
                _casinoBalanceManager,
                _gameBalanceManager,
                _logger.Object);
        }
    }
}
