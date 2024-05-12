using System.Collections.Generic;
using System.Linq;
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

        private ServiceProvider _serviceProvider = null;
        private GameLogicService _gameLogicService = null;
        private SceneLoaderService _sceneLoaderService = null;
        private SoundsPlayerService _soundsPlayerService = null;
        
        private Queue<CardView> _touchedCardQueue = new();
        
        protected override void OnInitialize(SceneLoaderService.GameSceneLaunchOptions data)
        {
            _serviceProvider = data.ServiceProvider;
            _gameLogicService = data.ServiceProvider.GetService<GameLogicService>();
            _sceneLoaderService = data.ServiceProvider.GetService<SceneLoaderService>();
            _soundsPlayerService = data.ServiceProvider.GetService<SoundsPlayerService>();
            
            if (!_gameLogicService.TryStartGameFromPersistence())
                _gameLogicService.StartGame(data.GameLevelSettings.Value);
            
            for (int col = 0; col < _gameLogicService.ColumnsNumber; ++col)
            {
                for (int row = 0; row < _gameLogicService.RowsNumber; ++row)
                {
                    var cardView = Instantiate(_cardViewPrefab, _cardsViewContainer);
                    cardView.Initialize(new CardView.CardViewData
                    {
                        Card = _gameLogicService.GetCard(col, row),
                        ServiceProvider = _serviceProvider,
                    });
                }
            }
            _cardViewPrefab.gameObject.SetActive(false);
            
            _gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayout.constraintCount = _gameLogicService.ColumnsNumber;
        }

        protected override void OnDeinitialize() {}
        
        public void OnCardViewTouched(CardView cardView)
        {
            _touchedCardQueue.Enqueue(cardView);
        }
        
        public void OnCheckMatch()
        {
            while (_touchedCardQueue.Count >= 2)
            {
                if (_touchedCardQueue.Take(2).Any((cardView) => cardView.IsAnimated))
                    return;

                var cardView1 = _touchedCardQueue.Dequeue();
                var cardView2 = _touchedCardQueue.Dequeue();

                if (cardView1.Data.Card.Symbol == cardView2.Data.Card.Symbol)
                    OnMatch(cardView1, cardView2);
                else
                    OnMissMatch(cardView1, cardView2);
            }
        }
        
        private void OnMatch(CardView cardView1, CardView cardView2)
        {
            Debug.Log("On Match");
            
            cardView1.OnMatch();
            cardView2.OnMatch();
            
            _gameLogicService.OnMatch();
            
            if (_gameLogicService.IsFinished)
            {
                _soundsPlayerService.PlayOneShot("GameOver");
                
                _sceneLoaderService.Launch(new SceneLoaderService.MenuSceneLaunchOptions
                {
                    ServiceProvider = _serviceProvider,
                });
                _gameLogicService.FinishGame();
            }
            else
            {
                _soundsPlayerService.PlayOneShot("Match");
            }
        }

        private void OnMissMatch(CardView cardView1, CardView cardView2)
        {
            Debug.Log("On MissMatch");
            
            cardView1.OnMissMatch();
            cardView2.OnMissMatch();
            
            _gameLogicService.OnMissMatch();
            
            _soundsPlayerService.PlayOneShot("Miss");
        }
    }
}