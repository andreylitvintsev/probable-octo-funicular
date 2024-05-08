using Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class LevelsMenuView : MonoBehaviour
    {
        [SerializeField] private Button _buttonPrefab = null;
        [SerializeField] private Transform _buttonContainer = null;
        
        private SceneLoaderService.MenuSceneLaunchOptions _launchOptions;
        
        public void Initialize(SceneLoaderService.MenuSceneLaunchOptions launchOptions)
        {
            _launchOptions = launchOptions;

            var gameLevelsService = _launchOptions.ServiceProvider.GetService<GameLevelsService>();
            var sceneLoaderService = _launchOptions.ServiceProvider.GetService<SceneLoaderService>();
            
            foreach (var gameLevelSettings in gameLevelsService.LevelSettings)
            {
                var button = Instantiate(_buttonPrefab, _buttonContainer);
                button.GetComponentInChildren<TMP_Text>().text = gameLevelSettings.Name;
                button.onClick.AddListener(() =>
                {
                    sceneLoaderService.Launch(new SceneLoaderService.GameSceneLaunchOptions 
                    {
                        ServiceProvider = _launchOptions.ServiceProvider,
                        GameLevelSettings = gameLevelSettings,
                    });
                });
            }

            _buttonPrefab.gameObject.SetActive(false);
        }
    }
}