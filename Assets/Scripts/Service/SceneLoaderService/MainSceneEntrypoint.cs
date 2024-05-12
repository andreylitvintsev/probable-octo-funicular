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
            _serviceProvider.GetService<SceneLoaderService>().Launch(new SceneLoaderService.MenuSceneLaunchOptions
            {
                ServiceProvider = _serviceProvider,
            });
        }
    }
}