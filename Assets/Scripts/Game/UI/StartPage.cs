using Framework.Localization;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class StartPage : Page<PageModel>
    {
        [SerializeField] private Text _bestScoreText;

        public override void OnEnter()
        {
            base.OnEnter();
            
            if (GameData.Data.BestScore > 0)
            {
                _bestScoreText.text = string.Format(LocalizationManager.GetString("best_score"), GameData.Data.BestScore);
                _bestScoreText.gameObject.SetActive(true);
            }
            else
            {
                _bestScoreText.gameObject.SetActive(false);
            }
        }

        [UsedImplicitly]
        public void StartPlay()
        {
            GameController.Instance.SetState(GameState.Play);
        }
    }
}