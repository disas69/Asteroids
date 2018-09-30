using Framework.Localization;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameOverPage : Page<PageModel>
    {
        [SerializeField] private Text _currentScoreText;
        [SerializeField] private Text _bestScoreText;

        public override void OnEnter()
        {
            base.OnEnter();
            
            _currentScoreText.text = string.Format(LocalizationManager.GetString("score"), GameData.Data.CurrentScore);
            _bestScoreText.text = string.Format(LocalizationManager.GetString("best_score"), GameData.Data.BestScore);
        }

        [UsedImplicitly]
        public void Replay()
        {
            GameController.Instance.SetState(GameState.Play);
        }

        [UsedImplicitly]
        public void Exit()
        {
            GameController.Instance.SetState(GameState.Idle);
        }
    }
}