using System;
using Framework.Extensions;
using UnityEngine;

namespace Game.Gameplay.SpaceObjects
{
    public enum SpaceObjectType
    {
        Ship,
        Asteroid,
        Laser
    }

    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D), typeof(SpaceObjectContactHandler))]
    public abstract class SpaceObject : MonoBehaviour
    {
        private SpaceObjectContactHandler _contactHandler;

        [SerializeField] private GameObject _view;
        [SerializeField] private ParticleSystem _destroyEffect;

        protected Collider2D Collider;
        protected Rigidbody2D Rigidbody;

        public event Action<SpaceObject> Deactivated;

        public abstract SpaceObjectType Type { get; }
        public bool IsActive { get; private set; }

        protected virtual void Awake()
        {
            _contactHandler = GetComponent<SpaceObjectContactHandler>();
            _contactHandler.Entered += OnContactHandlerEntered;
            _contactHandler.Exited += OnContactHandlerExited;

            Collider = GetComponent<Collider2D>();
            Rigidbody = GetComponent<Rigidbody2D>();
            
            Activate();
        }

        protected virtual void Activate()
        {
            IsActive = true;
            Collider.enabled = true;

            _view.gameObject.SetActive(true);
            _destroyEffect.gameObject.SetActive(false);
        }

        protected virtual void OnContactHandlerEntered(SpaceObject spaceObject)
        {
        }

        protected virtual void OnContactHandlerExited(SpaceObject spaceObject)
        {
        }

        protected virtual void Update()
        {
        }

        protected virtual void LateUpdate()
        {
            ClampPosition(SpaceBounds.Instance);
        }

        protected virtual void Deactivate(bool playDestroyEffect = true)
        {
            IsActive = false;
            Collider.enabled = false;
            Rigidbody.velocity = Vector2.zero;
            
            _view.gameObject.SetActive(false);

            if (playDestroyEffect)
            {
                _destroyEffect.gameObject.SetActive(true);
                _destroyEffect.Play();
                
                this.WaitForSeconds(_destroyEffect.main.duration, () =>
                {
                    Deactivated.SafeInvoke(this);
                });
            }
            else
            {
                Deactivated.SafeInvoke(this);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_contactHandler != null)
            {
                _contactHandler.Entered -= OnContactHandlerEntered;
                _contactHandler.Exited -= OnContactHandlerExited;
            }
        }

        private void ClampPosition(SpaceBounds spaceBounds)
        {
            var position = transform.position;
            if (position.x > spaceBounds.RightBound.x)
            {
                position.x = spaceBounds.LeftBound.x;
            }

            if (position.x < spaceBounds.LeftBound.x)
            {
                position.x = spaceBounds.RightBound.x;
            }

            if (position.y > spaceBounds.TopBound.y)
            {
                position.y = spaceBounds.BottomBound.y;
            }

            if (position.y < spaceBounds.BottomBound.y)
            {
                position.y = spaceBounds.TopBound.y;
            }

            transform.position = position;
        }
    }
}