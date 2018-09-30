using System.Collections;
using Framework.Extensions;
using Framework.Localization;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using Game.Configuration;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class PlayPage : Page<PageModel> 
	{
		private Coroutine _overlayTransitionCoroutine;

		[SerializeField] private Text _scoreText;
		[SerializeField] private Text _livesText;
		[SerializeField] private float _overlayTransitionSpeed;
		[SerializeField] private CanvasGroup _overlay;
		[SerializeField] private GameObject _mobileInput;

		public override void OnEnter()
		{
			base.OnEnter();
			
			OnScoreChanged(0);
			OnLivesChanged(GameConfiguration.Instance.LivesCount);
			
#if UNITY_IOS || UNITY_ANDROID
      		_mobileInput.SetActive(true);
#else
			_mobileInput.SetActive(false);
#endif
		}

		protected override IEnumerator InTransition()
		{
			_overlayTransitionCoroutine = StartCoroutine(ShowOverlay());
			yield return _overlayTransitionCoroutine;
		}

		private IEnumerator ShowOverlay()
		{
			_overlay.gameObject.SetActive(true);
			_overlay.alpha = 1f;

			while (_overlay.alpha > 0f)
			{
				_overlay.alpha -= _overlayTransitionSpeed * 2f * Time.deltaTime;
				yield return null;
			}

			_overlay.alpha = 0f;
			_overlay.gameObject.SetActive(false);
			_overlayTransitionCoroutine = null;
		}

		[UsedImplicitly]
		public void OnScoreChanged(int score)
		{
			_scoreText.text = string.Format(LocalizationManager.GetString("score"), score);
		}
		
		[UsedImplicitly]
		public void OnLivesChanged(int lives)
		{
			_livesText.text = string.Format(LocalizationManager.GetString("lives"), lives);
		}

		public override void OnExit()
		{
			_overlay.gameObject.SetActive(false);
			this.SafeStopCoroutine(_overlayTransitionCoroutine);
			base.OnExit();
		}
	}
}
