using System.Linq;
using ReactiveProperty;
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

            private bool _isMatched = false;
            public bool IsMatched => _isMatched;

            public Card(GameLogicService gameLogicService, int symbol)
            {
                _gameLogicService = gameLogicService;
                _symbol = symbol;
            }

            public void MarkAsMatched()
            {
                _isMatched = true;
            }
        }
        
        private GameLevelsService.GameLevelSettings? _gameLevelSettings = null;
        private Card[] _cards = null;

        public bool IsFinished => _gameLevelSettings == null ||
                                  _cards == null ||
                                  _cards.All(card => card.IsMatched);

        private readonly ReactiveProperty<int> _turns = new();
        public IReadOnlyReactiveProperty<int> Turns => _turns;

        public string Name => _gameLevelSettings.Value.Name;
        public int ColumnsNumber => _gameLevelSettings.Value.ColumnsNumber;
        public int RowsNumber => _gameLevelSettings.Value.RowsNumber;
        
        public bool HasPersistedState => GameLogicServicePersistenceHelper.HasPersistedState();

        protected override void OnInitialize() {}

        protected override void OnDeinitialize()
        {
            if (!IsFinished)
                GameLogicServicePersistenceHelper.Persist(this);
        }

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

        public bool TryStartGameFromPersistence()
        {
            var persistedGameState = GameLogicServicePersistenceHelper.Load();
            if (persistedGameState == null)
                return false;

            _gameLevelSettings = new GameLevelsService.GameLevelSettings
            {
                Name = persistedGameState.Value.Name,
                ColumnsNumber = persistedGameState.Value.ColumnsNumber,
                RowsNumber = persistedGameState.Value.RowsNumber,
            };
            _cards = persistedGameState.Value.Cards.Select((persistedCard) =>
            {
                var card = new Card(this, persistedCard.Symbol);
                if (persistedCard.IsMatched)
                    card.MarkAsMatched();
                return card;
            }).ToArray();
            _turns.Value = persistedGameState.Value.TurnsNumber;
            
            return true;
        }

        public void FinishGame()
        {
            _gameLevelSettings = null;
            _cards = null;
            _turns.Value = 0;
            GameLogicServicePersistenceHelper.Clear();
        }
        
        public Card GetCard(int col, int row)
        {
            if (!_gameLevelSettings.HasValue)
                return null;
            return _cards[_gameLevelSettings.Value.ColumnsNumber * row + col];
        }

        public void OnMatch()
        {
            _turns.Value += 1;
        }

        public void OnMissMatch()
        {
            _turns.Value += 1;
        }
    }
}