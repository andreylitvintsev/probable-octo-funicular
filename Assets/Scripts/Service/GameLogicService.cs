using System;
using Random = UnityEngine.Random;

namespace Service
{
    public class GameLogicService : AbstractService
    {
        public class Card
        {
            private GameLogicService _gameLogicService = null;
            
            private int _symbol = 0;
            public int Symbol => _symbol;

            public Card(GameLogicService gameLogicService, int symbol)
            {
                _gameLogicService = gameLogicService;
                _symbol = symbol;
            }
        }
        
        private GameLevelsService.GameLevelSettings? _gameLevelSettings = null;
        private Card[] _cards = null;

        public override void Initialize() {}

        private static void Shuffle(Card[] array)
        {
            for (var t = 0; t < array.Length; t++)
            {
                var tmp = array[t];
                var r = Random.Range(t, array.Length);
                array[t] = array[r];
                array[r] = tmp;
            }
        }

        public void StartGame(GameLevelsService.GameLevelSettings gameLevelSettings)
        {
            _gameLevelSettings = gameLevelSettings;
            _cards = new Card[gameLevelSettings.ColumnsNumber * gameLevelSettings.RowsNumber];
            
            var currentSymbol = 0;
            for (var i = 0; i < _cards.Length; i += 2)
            {
                _cards[i] = new Card(this, currentSymbol);
                _cards[i + 1] = new Card(this, currentSymbol);
                ++currentSymbol;
            }
            Shuffle(_cards);
        }

        public void FinishGame()
        {
            _gameLevelSettings = null;
            _cards = null;
        }
        
        public Card GetCard(int col, int row)
        {
            if (!_gameLevelSettings.HasValue)
                return null;
            return _cards[_gameLevelSettings.Value.ColumnsNumber * row + col];
        }
    }
}