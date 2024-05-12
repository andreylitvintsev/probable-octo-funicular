using System;
using UnityEngine;

namespace Service
{
    public static class GameLogicServicePersistenceHelper
    {
        [Serializable]
        public struct PersistedCard
        {
            public int Symbol;
            public bool IsMatched;
        }

        public struct PersistedGameState
        {
            public string Name;
            public int TurnsNumber;
            public PersistedCard[] Cards;
            public int ColumnsNumber;
            public int RowsNumber;
        }

        private const string PlayerPrefsKey = "PesistedGame";
        
        public static void Persist(GameLogicService gameLogicService)
        {
            if (gameLogicService.IsFinished)
                throw new Exception("Game must be started!");

            var persistedCards = new PersistedCard[gameLogicService.ColumnsNumber * gameLogicService.RowsNumber];
            int persistedCardsIndex = 0;
            for (int row = 0; row < gameLogicService.RowsNumber; ++row)
            {
                for (int column = 0; column < gameLogicService.ColumnsNumber; ++column)
                {
                    var card = gameLogicService.GetCard(column, row);
                    persistedCards[persistedCardsIndex] = new PersistedCard
                    {
                        IsMatched = card.IsMatched,
                        Symbol = card.Symbol,
                    };
                    ++persistedCardsIndex;
                }
            }

            var persistedGameState = new PersistedGameState
            {
                TurnsNumber = gameLogicService.Turns.Value,
                Cards = persistedCards,
                ColumnsNumber = gameLogicService.ColumnsNumber,
                RowsNumber = gameLogicService.RowsNumber,
            };
            
            PlayerPrefs.SetString(PlayerPrefsKey, JsonUtility.ToJson(persistedGameState));
            PlayerPrefs.Save();
        }

        public static PersistedGameState? Load()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsKey))
                return null;

            return JsonUtility.FromJson<PersistedGameState>(PlayerPrefs.GetString(PlayerPrefsKey));
        }

        public static bool HasPersistedState()
        {
            return PlayerPrefs.HasKey(PlayerPrefsKey);
        }

        public static void Clear()
        {
            PlayerPrefs.DeleteKey(PlayerPrefsKey);
            PlayerPrefs.Save();
        }
    }
}