using Context.Interfaces;
using Betting.Data;
using Context;

namespace Betting
{
    public class BettingModule : IDisposableObject
    {
        private readonly BettingController _bettingController;
        private readonly ChipsController _chipsController;

        public BettingModule(DataStore dataStore)
        {
            _bettingController = new BettingController(dataStore);
            _chipsController = new ChipsController();
        }

        public void Dispose()
        {
            _bettingController.Dispose();
            _chipsController.Dispose();
        }

        public void ResolveBets(int resultNumber)
        {
            _bettingController.ResolveBets(resultNumber);
        }
    }
}