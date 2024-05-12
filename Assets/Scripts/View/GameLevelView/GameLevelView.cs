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
            
            _gameLogicService.StartGame(data.GameLevelSettings);
            
            for (int col = 0; col < data.GameLevelSettings.ColumnsNumber; ++col)
            {
                for (int row = 0; row < data.GameLevelSettings.RowsNumber; ++row)
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
            _gridLayout.constraintCount = data.GameLevelSettings.ColumnsNumber;
        }

        protected override void OnDeinitialize()
        {
            _gameLogicService.FinishGame();
        }
        
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
            
            if (_gameLogicService.IsFinished)
            {
                _soundsPlayerService.PlayOneShot("GameOver");
                
                _sceneLoaderService.Launch(new SceneLoaderService.MenuSceneLaunchOptions
                {
                    ServiceProvider = _serviceProvider,
                });
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
            
            _soundsPlayerService.PlayOneShot("Miss");
        }
    }
}