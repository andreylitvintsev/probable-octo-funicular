using UnityEngine;

namespace Service
{
    public class MainSceneEntrypoint : MonoBehaviour
    {
        [SerializeField] private ServiceProvider _serviceProvider;
        
        private void Awake()
        {
            DontDestroyOnLoad(_serviceProvider);
            _serviceProvider.Initialize();
            _serviceProvider.GetService<SceneLoaderService>().Launch(new SceneLoaderService.MenuSceneLaunchOptions
            {
                ServiceProvider = _serviceProvider,
            });
        }
    }
}