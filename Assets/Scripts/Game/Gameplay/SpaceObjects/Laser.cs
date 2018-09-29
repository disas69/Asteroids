using System.Collections;
using UnityEngine;

namespace Game.Gameplay.SpaceObjects
{
    public class Laser : SpaceObject
    {
        private Coroutine _expirationCoroutine;
        
        [SerializeField] private float _velocity;
        [SerializeField] private float _expirationTime;

        public override SpaceObjectType Type
        {
            get { return SpaceObjectType.Laser; }
        }

        protected override void OnContactHandlerEntered(SpaceObject spaceObject)
        {
            base.OnContactHandlerEntered(spaceObject);

            if (_expirationCoroutine != null)
            {
                StopCoroutine(_expirationCoroutine);
            }
            
            Deactivate(false);
        }

        public void Fire(Vector2 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
            
            Activate();
            Rigidbody.AddForce(transform.up * _velocity);
            
            _expirationCoroutine = StartCoroutine(Expire());
        }

        private IEnumerator Expire()
        {
            yield return new WaitForSeconds(_expirationTime);
            Deactivate(false);
        }
    }
}