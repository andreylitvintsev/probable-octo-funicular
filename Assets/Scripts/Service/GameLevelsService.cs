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
            [field: SerializeField] public string Name { get; private set; }
            [field: SerializeField, Min(2)] public int ColumnsNumber { get; private set; }
            [field: SerializeField, Min(2)] public int RowsNumber { get; private set; }
        }
        
        [SerializeField] private List<GameLevelSettings> _levelSettings = null;
        public IReadOnlyList<GameLevelSettings> LevelSettings => _levelSettings;

        public override void Initialize() {}
    }
}