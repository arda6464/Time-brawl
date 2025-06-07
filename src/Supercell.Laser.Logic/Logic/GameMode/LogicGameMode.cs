using System;
using System.Collections.Generic;
using Supercell.Laser.Logic.Battle.Structures;
using Supercell.Laser.Logic.Avatar;

namespace Supercell.Laser.Logic.Logic.GameMode
{
    public enum GameState
    {
        Waiting,
        Playing,
        Ended
    }

    public class LogicGameMode
    {
        private List<BattlePlayer> _players = new List<BattlePlayer>();
        private GameState _gameState = GameState.Playing;

        public void EndGame()
        {
            if (_gameState == GameState.Ended) return;
            _gameState = GameState.Ended;

            // Normal oyun sonu işlemleri
            foreach (var player in _players)
            {
                if (player != null)
                {
                    var avatar = player.Avatar;
                    if (avatar != null)
                    {
                        // ... existing code ...
                    }
                }
            }
        }
    }
} 