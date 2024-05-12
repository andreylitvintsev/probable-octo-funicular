using ReactiveProperty;
using Service;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class GameHudView : AbstractView<SceneLoaderService.GameSceneLaunchOptions>
    {
        [SerializeField] private Button _backButton = null;

        private SceneLoaderService _sceneLoaderService = null;
        
        protected override void OnInitialize(SceneLoaderService.GameSceneLaunchOptions data)
        {
            _sceneLoaderService = data.ServiceProvider.GetService<SceneLoaderService>();
            
            _backButton.SubscribeOnClick(() =>
            {
                _sceneLoaderService.Launch(new SceneLoaderService.MenuSceneLaunchOptions
                {
                    ServiceProvider = data.ServiceProvider,
                });
            }).DisposeWith(this);
        }

        protected override void OnDeinitialize() {}
    }
}