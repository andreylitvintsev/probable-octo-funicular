using UnityEngine;

namespace Service
{
    public class MainSceneEntrypoint : MonoBehaviour
    {
        [SerializeField] private ServiceProvider _serviceProvider = null;
        
        private void Awake()
        {
            DontDestroyOnLoad(_serviceProvider);

            Debug.unityLogger.logEnabled = Application.isEditor || Debug.isDebugBuild;
            
            _serviceProvider.Initialize();
            var sceneLoaderService = _serviceProvider.GetService<SceneLoaderService>();
            var gameLogicService = _serviceProvider.GetService<GameLogicService>();

            if (gameLogicService.HasPersistedState)
            {
                sceneLoaderService.Launch(new SceneLoaderService.GameSceneLaunchOptions
                {
                    ServiceProvider = _serviceProvider,
                });
            }
            else
            {
                sceneLoaderService.Launch(new SceneLoaderService.MenuSceneLaunchOptions
                {
                    ServiceProvider = _serviceProvider,
                });
            }
        }
    }
}