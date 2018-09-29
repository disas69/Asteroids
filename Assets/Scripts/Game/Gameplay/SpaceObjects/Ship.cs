using System.Collections.Generic;
using Framework.Extensions;
using Game.Gameplay.Spawn;
using UnityEngine;

namespace Game.Gameplay.SpaceObjects
{
    public class Ship : SpaceObject
    {
        private float _timeFromLastShot;
        private List<SpaceObject> _activeLasers;

        [SerializeField] private float _shotDelay;
        [SerializeField] private float _rotationSpeed = 100.0f;
        [SerializeField] private float _thrustForce = 3f;
        [SerializeField] private Transform _weapon;
        [SerializeField] private Spawner _laserSpawner;

        public override SpaceObjectType Type
        {
            get { return SpaceObjectType.Ship; }
        }

        protected override void Awake()
        {
            base.Awake();
            _activeLasers = new List<SpaceObject>();
            _laserSpawner.transform.SetParent(transform.parent);
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
            Deactivate();
        }

        protected override void Update()
        {
            base.Update();

            if (IsActive)
            {
                transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * _rotationSpeed * Time.deltaTime);
                Rigidbody.AddForce(transform.up * _thrustForce * Input.GetAxis("Vertical"));

                if (Input.GetKey(KeyCode.Space))
                {
                    if (_timeFromLastShot > _shotDelay)
                    {
                        var laser = _laserSpawner.Spawn() as Laser;
                        if (laser != null)
                        {
                            _activeLasers.Add(laser);
                            laser.Deactivated += OnLaserDeactivated;
                            laser.Fire(_weapon.transform.position, transform.rotation);
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

        protected override void Deactivate(bool playDestroyEffect = true)
        {
            base.Deactivate(playDestroyEffect);

            for (int i = 0; i < _activeLasers.Count; i++)
            {
                _laserSpawner.Despawn(_activeLasers[i]);
            }
            
            _activeLasers.Clear();

            this.WaitForSeconds(2f, Activate);
        }

        private void OnLaserDeactivated(SpaceObject laser)
        {
            laser.Deactivated -= OnLaserDeactivated;
            
            _activeLasers.Remove(laser);
            _laserSpawner.Despawn(laser);
        }
    }
}