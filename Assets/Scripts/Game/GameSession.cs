using Framework.Extensions;
using Framework.Signals;
using Game.Configuration;
using Game.Gameplay.SpaceObjects;
using Game.Gameplay.Spawn;
using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    public class GameSession : MonoBehaviour
    {
        private int _lives;
        private int _score;

        [SerializeField] private Spawner _shipSpawner;
        [SerializeField] private AsteroidsSpawner _asteroidsSpawner;
        [SerializeField] private Signal _gameStartedSignal;
        [SerializeField] private Signal _scoreChangedSignal;
        [SerializeField] private Signal _livesCountChangedSignal;

        public void Activate()
        {
            if (_asteroidsSpawner.Spawning)
            {
                return;
            }
            
            _asteroidsSpawner.StartSpawn();
        }

        public void StartSession()
        {
            _lives = GameConfiguration.Instance.LivesCount;
            _score = 0;
            StartGame();
        }

        public void StopSession()
        {
            GameData.Data.CurrentScore = _score;
            if (_score > GameData.Data.BestScore)
            {
                GameData.Data.BestScore = _score;
                GameData.Save();
            }
        }

        [UsedImplicitly]
        public void OnShipDestroyed()
        {
            _shipSpawner.Flush();
            _lives--;

            if (_lives > 0)
            {
                SignalsManager.Broadcast(_livesCountChangedSignal.Name, _lives);
                this.WaitForSeconds(GameConfiguration.Instance.DelayBetweenSessions, StartGame);
            }
            else
            {
                GameController.Instance.SetState(GameState.GameOver);
            }
        }

        [UsedImplicitly]
        public void OnAsteroidDestroyed(int scorePoints)
        {
            _score += scorePoints;
            SignalsManager.Broadcast(_scoreChangedSignal.Name, _score);
        }

        private void StartGame()
        {
            _asteroidsSpawner.RestartSpawn();
            
            var spaceShip = _shipSpawner.Spawn() as Ship;
            if (spaceShip != null)
            {
                spaceShip.Setup(ShipSettings.Instance);
            }
            
            SignalsManager.Broadcast(_gameStartedSignal.Name);
        }
    }
}