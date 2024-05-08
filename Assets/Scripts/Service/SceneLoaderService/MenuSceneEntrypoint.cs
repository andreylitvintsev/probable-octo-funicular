using UnityEngine;
using View;

namespace Service
{
    public class MenuSceneEntrypoint : MonoBehaviour
    {
        [SerializeField] private LevelsMenuView _menuLevelView = null;
        
        public void Initialize(SceneLoaderService.MenuSceneLaunchOptions launchOptions)
        {
            _menuLevelView.Initialize(launchOptions);
        }
    }
}