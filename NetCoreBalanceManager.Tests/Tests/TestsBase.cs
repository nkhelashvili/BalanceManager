using Balances;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreBalanceManager.Tests.Tests
{
    public class TestsBase
    {
        protected CasinoBalanceManager _casinoBalanceManager;
        protected GameBalanceManager _gameBalanceManager;
        protected IBalanceManager _balanceManager;

        public virtual void SetUp()
        {
            _casinoBalanceManager = new CasinoBalanceManager();
            _gameBalanceManager = new GameBalanceManager();
        }
    }
}
