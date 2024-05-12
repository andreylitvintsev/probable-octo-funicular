using UnityEngine;
using View;

namespace Service
{
    public class GameSceneEntrypoint : MonoBehaviour
    {
        [SerializeField] private GameLevelView _gameLevelView = null;
        [SerializeField] private GameHudView _gameHudView = null;
        
        public void Initialize(SceneLoaderService.GameSceneLaunchOptions launchOptions)
        {
            _gameLevelView.Initialize(launchOptions);
            _gameHudView.Initialize(launchOptions);
        }
    }
}