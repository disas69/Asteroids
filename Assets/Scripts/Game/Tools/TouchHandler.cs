using Framework.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Game.Tools
{
    [RequireComponent(typeof(Image))]
    public class TouchHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private TapDetector _tapDetector;

        [SerializeField] private UnityEvent _onTapEvent;
        [SerializeField] private UnityEvent _onHoldEvent;

        private void Awake()
        {
            _tapDetector = new TapDetector(0.5f, 0.5f);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!SplashScreen.isFinished)
            {
                return;
            }

            _tapDetector.RegisterTouchDown(eventData);
        }

        public void Update()
        {
            var result = _tapDetector.Update();
            if (result == TouchResult.Hold)
            {
                _onHoldEvent.Invoke();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            var result = _tapDetector.RegisterTouchUp(eventData);
            if (result == TouchResult.Tap)
            {
                _onTapEvent.Invoke();
            }
        }
    }
}