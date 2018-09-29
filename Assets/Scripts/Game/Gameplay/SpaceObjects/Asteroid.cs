using Game.Configuration;
using Game.Gameplay.Spawn;
using UnityEngine;

namespace Game.Gameplay.SpaceObjects
{
    public enum AsteroidType
    {
        Small,
        Medium,
        Big
    }
    
    [RequireComponent(typeof(Spawner))]
    public class Asteroid : SpaceObject
    {
        private Spawner _spawner;
        private Vector2 _direction;
        private float _initialVelocity;
        private float _maxVelocity;
        private float _maxTorque;
        private SpawnSettings _spawnSettings;

        [SerializeField] private AsteroidType _asteroidType;

        public override SpaceObjectType Type
        {
            get { return SpaceObjectType.Asteroid; }
        }
        
        public AsteroidType AsteroidType
        {
            get { return _asteroidType; }
        }

        public void Push(AsteroidSettings asteroidSettings)
        {
            _initialVelocity = asteroidSettings.InitialVelocity;
            _maxVelocity = asteroidSettings.MaxVelocity;
            _maxTorque = asteroidSettings.MaxTorque;

            if (asteroidSettings.SpawnOnDestroy)
            {
                _spawnSettings = asteroidSettings.SpawnOnDestroySettings;
                _spawner.Activate(_spawnSettings);
            }
            else
            {
                _spawnSettings = null;
            }
            
            Activate();
        }

        protected override void Awake()
        {
            base.Awake();
            
            _spawner = GetComponent<Spawner>();
        }

        protected override void Activate()
        {
            base.Activate();
            
            _direction = new Vector2(Random.value, Random.value);
            transform.position = CalculatePosition(SpaceBounds.Instance);
            Rigidbody.velocity = new Vector2(Random.Range(0f, _initialVelocity), Random.Range(0f, _initialVelocity));
            Rigidbody.AddTorque(_maxTorque);
        }

        private Vector2 CalculatePosition(SpaceBounds spaceBounds)
        {
            var position = new Vector2
            {
                x = Random.value > 0.5f ? spaceBounds.RightBound.x : spaceBounds.LeftBound.x,
                y = Random.Range(spaceBounds.TopBound.y, spaceBounds.BottomBound.y)
            };

            return position;
        }

        protected override void OnContactHandlerEntered(SpaceObject spaceObject)
        {
            if (spaceObject.Type == SpaceObjectType.Asteroid)
            {
                return;
            }

            Deactivate();
        }

        protected override void Deactivate(bool playDestroyEffect = true)
        {
            if (_spawnSettings != null)
            {
                for (int i = 0; i < _spawnSettings.Count; i++)
                {
                    var asteroid = _spawner.Spawn() as Asteroid;
                    if (asteroid != null)
                    {
                        asteroid.Deactivated += OnSpawnedAsteroidDeactivated;
                        asteroid.Push(AsteroidsConfiguration.Instance.GetAsteroidSettings(asteroid.AsteroidType));
                        asteroid.transform.position = transform.position;
                        asteroid.transform.SetParent(transform.parent);
                    }
                }
            }
            
            base.Deactivate(playDestroyEffect);
        }

        private void OnSpawnedAsteroidDeactivated(SpaceObject asteroid)
        {
            asteroid.Deactivated -= OnSpawnedAsteroidDeactivated;
            Destroy(asteroid.gameObject);
        }

        protected override void Update()
        {
            base.Update();

            if (Rigidbody.velocity.magnitude < _maxVelocity)
            {
                Rigidbody.AddRelativeForce(_direction);
            }
        }
    }
}