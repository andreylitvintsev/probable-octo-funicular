using ReactiveProperty;
using Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class LevelsMenuView : AbstractView<SceneLoaderService.MenuSceneLaunchOptions>
    {
        [SerializeField] private Button _buttonPrefab = null;
        [SerializeField] private Transform _buttonContainer = null;
        
        protected override void OnInitialize(SceneLoaderService.MenuSceneLaunchOptions data)
        {
            var gameLevelsService = Data.ServiceProvider.GetService<GameLevelsService>();
            var sceneLoaderService = Data.ServiceProvider.GetService<SceneLoaderService>();
            
            foreach (var gameLevelSettings in gameLevelsService.LevelSettings)
            {
                var button = Instantiate(_buttonPrefab, _buttonContainer);
                button.GetComponentInChildren<TMP_Text>().text = gameLevelSettings.Name;
                button.SubscribeOnClick(() =>
                {
                    sceneLoaderService.Launch(new SceneLoaderService.GameSceneLaunchOptions 
                    {
                        ServiceProvider = Data.ServiceProvider,
                        GameLevelSettings = gameLevelSettings,
                    });
                }).DisposeWith(this);
            }

            _buttonPrefab.gameObject.SetActive(false);
        }

        protected override void OnDeinitialize() {}
    }
}