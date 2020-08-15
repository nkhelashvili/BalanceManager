using Balances;

namespace NetCoreBalanceManager.Tests.Tests
{
    public class TestsBase
    {
        protected CasinoBalanceManager _casinoBalanceManager;
        protected GameBalanceManager _gameBalanceManager;
        protected IBalanceManager _balanceManager;

        public TestsBase()
        {
            _casinoBalanceManager = new CasinoBalanceManager();
            _gameBalanceManager = new GameBalanceManager();
        }
    }
}
