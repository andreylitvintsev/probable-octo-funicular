using System;
using System.Collections.Generic;
using UnityEngine;

namespace Service
{
    public class GameLevelsService : AbstractService
    {
        [Serializable]
        public struct GameLevelSettings
        {
            [SerializeField] public string Name;
            [SerializeField, Min(2)] public int ColumnsNumber;
            [SerializeField, Min(2)] public int RowsNumber;
        }
        
        [SerializeField] private List<GameLevelSettings> _levelSettings = null;
        public IReadOnlyList<GameLevelSettings> LevelSettings => _levelSettings;

        protected override void OnInitialize() {}
        protected override void OnDeinitialize() {}
    }
}