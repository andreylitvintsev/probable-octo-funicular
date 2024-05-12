using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Service
{
    public class SceneLoaderService : AbstractService
    {
        public struct MenuSceneLaunchOptions
        {
            public ServiceProvider ServiceProvider;
        }

        public struct GameSceneLaunchOptions
        {
            public ServiceProvider ServiceProvider;
            public GameLevelsService.GameLevelSettings? GameLevelSettings;
        }

        protected override void OnInitialize() {}

        protected override void OnDeinitialize() {}

        public void Launch(MenuSceneLaunchOptions launchOptions, Action<MenuSceneEntrypoint> onLaunchedScene = null)
        {
            LoadScene<MenuSceneEntrypoint>("MenuScene", bootstrap =>
            {
                bootstrap.Initialize(launchOptions);
                onLaunchedScene?.Invoke(bootstrap);
            });
        }
        
        public void Launch(GameSceneLaunchOptions launchOptions, Action<GameSceneEntrypoint> onLaunchedScene = null)
        {
            LoadScene<GameSceneEntrypoint>("GameScene", bootstrap =>
            {
                bootstrap.Initialize(launchOptions);
                onLaunchedScene?.Invoke(bootstrap);
            });
        }

        private void LoadScene<TBootstrap>(string sceneName, Action<TBootstrap> setupSceneAction = null) where TBootstrap : MonoBehaviour
        {
            IEnumerator AsyncLogic()
            {
                // запускаем сцену локации
                yield return SceneManager.LoadSceneAsync(sceneName);

                // инициализируем сцену
                var locationBootstrap = FindObjectOfType<TBootstrap>();
                setupSceneAction?.Invoke(locationBootstrap);
            }

            StartCoroutine(AsyncLogic());
        }
    }
}