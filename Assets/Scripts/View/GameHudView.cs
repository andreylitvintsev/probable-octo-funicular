using System.Globalization;
using ReactiveProperty;
using Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class GameHudView : AbstractView<SceneLoaderService.GameSceneLaunchOptions>
    {
        [SerializeField] private Button _backButton = null;
        [SerializeField] private TMP_Text _turnsValueText = null;

        private SceneLoaderService _sceneLoaderService = null;
        private GameLogicService _gameLogicService = null;
        
        protected override void OnInitialize(SceneLoaderService.GameSceneLaunchOptions data)
        {
            _sceneLoaderService = data.ServiceProvider.GetService<SceneLoaderService>();
            _gameLogicService = data.ServiceProvider.GetService<GameLogicService>();
            
            _backButton.
                SubscribeOnClick(OnBackButtonClick)
                .DisposeWith(this);

            _gameLogicService.Turns
                .SubscribeOnValueChanged(OnTurnsUpdate, notifyOnSubscribe: true)
                .DisposeWith(this);
        }

        private void OnBackButtonClick()
        {
            _sceneLoaderService.Launch(new SceneLoaderService.MenuSceneLaunchOptions
            {
                ServiceProvider = Data.ServiceProvider,
            });
        }

        private void OnTurnsUpdate(int value)
        {
            _turnsValueText.text = value.ToString(NumberFormatInfo.InvariantInfo);
        }

        protected override void OnDeinitialize() {}
    }
}