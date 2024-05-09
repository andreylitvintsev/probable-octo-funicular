using Service;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class GameLevelView : AbstractView<SceneLoaderService.GameSceneLaunchOptions>
    {
        [SerializeField] private CardView _cardViewPrefab = null;
        [SerializeField] private Transform _cardsViewContainer = null;
        [SerializeField] private GridLayoutGroup _gridLayout;
        
        private GameLogicService _gameLogicService = null;
        
        protected override void OnInitialize(SceneLoaderService.GameSceneLaunchOptions data)
        {
            _gameLogicService = data.ServiceProvider.GetService<GameLogicService>();
            _gameLogicService.StartGame(data.GameLevelSettings);
            
            for (int col = 0; col < data.GameLevelSettings.ColumnsNumber; ++col)
            {
                for (int row = 0; row < data.GameLevelSettings.RowsNumber; ++row)
                {
                    var cardView = Instantiate(_cardViewPrefab, _cardsViewContainer);
                    cardView.Initialize(_gameLogicService.GetCard(col, row));
                }
            }
            _cardViewPrefab.gameObject.SetActive(false);
            
            _gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayout.constraintCount = data.GameLevelSettings.ColumnsNumber;
        }

        protected override void OnDeinitialize()
        {
            _gameLogicService.FinishGame();
        }
    }
}