using Framework.Tools.Gameplay;
using Framework.Tools.Singleton;
using Framework.UI;
using Game.UI;
using UnityEngine;

namespace Game
{
    public enum GameState
    {
        Idle,
        Play,
        GameOver
    }

    public class GameController : MonoSingleton<GameController>
    {
        private StateMachine<GameState> _gameStateMachine;

        [SerializeField] private GameSession _gameSession;
        [SerializeField] private UINavigation _pagesNavigation;

        private void Start()
        {
            _gameStateMachine = CreateStateMachine();
            GameData.Load();
            ActivateStart();
        }

        public GameState GetState()
        {
            return _gameStateMachine.GetCurrentState();
        }

        public void SetState(GameState gameState)
        {
            _gameStateMachine.SetState(gameState);
        }

        private StateMachine<GameState> CreateStateMachine()
        {
            var stateMachine = new StateMachine<GameState>(GameState.Idle);
            stateMachine.AddTransition(GameState.Idle, GameState.Play, ActivatePlay);
            stateMachine.AddTransition(GameState.Play, GameState.GameOver, ActivateGameOver);
            stateMachine.AddTransition(GameState.GameOver, GameState.Play, ActivatePlay);
            stateMachine.AddTransition(GameState.GameOver, GameState.Idle, ActivateStart);

            return stateMachine;
        }

        private void ActivateStart()
        {
            _gameSession.Activate();
            _pagesNavigation.OpenScreen<StartPage>();
        }

        private void ActivatePlay()
        {
            _gameSession.StartSession();
            _pagesNavigation.OpenScreen<PlayPage>();
        }

        private void ActivateGameOver()
        {
            _gameSession.StopSession();
            _pagesNavigation.OpenScreen<GameOverPage>();
        }

        private void OnDestroy()
        {
            GameData.Save();
        }
    }
}