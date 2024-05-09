using UnityEngine;
using View;

namespace Service
{
    public class GameSceneEntrypoint : MonoBehaviour
    {
        [SerializeField] private GameLevelView _gameLevelView = null;
        
        public void Initialize(SceneLoaderService.GameSceneLaunchOptions launchOptions)
        {
            _gameLevelView.Initialize(launchOptions);
        }
    }
}