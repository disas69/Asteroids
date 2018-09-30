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
            _pagesNavigation.OpenScreen<StartPage>();
            _gameSession.Activate();
        }

        private void ActivatePlay()
        {
            _pagesNavigation.OpenScreen<PlayPage>();
            _gameSession.StartSession();
        }

        private void ActivateGameOver()
        {
            _pagesNavigation.OpenScreen<GameOverPage>();
            _gameSession.StopSession();
        }

        private void OnDestroy()
        {
            GameData.Save();
        }
    }
}