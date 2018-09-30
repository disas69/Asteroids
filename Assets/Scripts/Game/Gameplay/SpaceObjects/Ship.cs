using Framework.Signals;
using Game.Configuration;
using Game.Gameplay.Spawn;
using Game.Input;
using UnityEngine;

namespace Game.Gameplay.SpaceObjects
{
    public class Ship : SpaceObject
    {
        private IInputProvider _input;
        private float _timeFromLastShot;
        private float _movingForce;
        private float _rotationSpeed;
        private float _shotDelay;

        [SerializeField] private Transform _weapon;
        [SerializeField] private Spawner _laserSpawner;
        [SerializeField] private Signal _destroyedSignal;

        public override SpaceObjectType Type
        {
            get { return SpaceObjectType.Ship; }
        }

        protected override void Awake()
        {
            base.Awake();
            _laserSpawner.transform.SetParent(transform.parent);
            
#if UNITY_IOS || UNITY_ANDROID
      		_input = new MobileInput();
#else
            _input = new KeyboardInput();
#endif
        }

        public void Setup(ShipSettings settings)
        {
            _movingForce = settings.MovingForce;
            _rotationSpeed = settings.RotationSpeed;
            _shotDelay = settings.ShotDelay;

            Rigidbody.drag = settings.LinearDrag;
            Rigidbody.angularDrag = settings.AngularDrag;

            Activate();
        }

        protected override void Activate()
        {
            base.Activate();
            transform.position = Vector3.zero;
            transform.eulerAngles = Vector3.zero;
        }

        protected override void OnContactHandlerEntered(SpaceObject spaceObject)
        {
            base.OnContactHandlerEntered(spaceObject);
            SignalsManager.Broadcast("PlayAudio", "ship_destroyed");
            Deactivate();
        }

        protected override void Update()
        {
            base.Update();
            if (IsActive)
            {
                transform.Rotate(0, 0, -_input.GetRotation() * _rotationSpeed * Time.deltaTime);
                Rigidbody.AddForce(transform.up * _movingForce * _input.GetMovement());

                if (_input.ActionButtonPressed())
                {
                    if (_timeFromLastShot > _shotDelay)
                    {
                        var laser = _laserSpawner.Spawn() as Laser;
                        if (laser != null)
                        {
                            laser.Setup(_weapon.transform.position, transform.rotation);
                            SignalsManager.Broadcast("PlayAudio", "laser");
                        }

                        _timeFromLastShot = 0f;
                    }
                    else
                    {
                        _timeFromLastShot += Time.deltaTime;
                    }
                }
                else
                {
                    _timeFromLastShot = float.MaxValue;
                }
            }
        }

        public override void Deactivate(bool destroy = true)
        {
            base.Deactivate(destroy);
            _laserSpawner.Flush();
        }

        protected override void FireDeactivated()
        {
            base.FireDeactivated();
            SignalsManager.Broadcast(_destroyedSignal.Name);
        }
    }
}