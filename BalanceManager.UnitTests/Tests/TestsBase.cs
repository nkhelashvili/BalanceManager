

using Balances;

namespace BalanceManager.UnitTests.Tests
{
    public class TestsBase
    {
        #region Private Fields
        protected CasinoBalanceManager _casinoBalanceManager;
        protected GameBalanceManager _gameBalanceManager;
        protected IBalanceManager _balanceManager;
        #endregion

        #region Constructors
        public TestsBase()
        {
            _casinoBalanceManager = new CasinoBalanceManager();
            _gameBalanceManager = new GameBalanceManager();
        }
        #endregion
    }
}
