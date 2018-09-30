using Framework.Signals;
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
        private int _scorePoints;

        [SerializeField] private AsteroidType _asteroidType;
        [SerializeField] private Signal _destroyedSignal;

        public override SpaceObjectType Type
        {
            get { return SpaceObjectType.Asteroid; }
        }
        
        public AsteroidType AsteroidType
        {
            get { return _asteroidType; }
        }

        public void Setup(AsteroidSettings asteroidSettings)
        {
            _initialVelocity = asteroidSettings.InitialVelocity;
            _maxVelocity = asteroidSettings.MaxVelocity;
            _maxTorque = asteroidSettings.MaxTorque;
            _scorePoints = asteroidSettings.ScorePoints;

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

            if (spaceObject.Type == SpaceObjectType.Laser)
            {
                SignalsManager.Broadcast(_destroyedSignal.Name, _scorePoints);
                SignalsManager.Broadcast("PlayAudio", "destroyed");
                Deactivate();
            }
        }

        public override void Deactivate(bool destroy = true)
        {
            if (destroy)
            {
                SpawnInnerAsteroids();
            }
            
            base.Deactivate(destroy);
        }

        protected override void Update()
        {
            base.Update();
            if (Rigidbody.velocity.magnitude < _maxVelocity)
            {
                Rigidbody.AddRelativeForce(_direction);
            }
        }

        private void SpawnInnerAsteroids()
        {
            if (_spawnSettings != null)
            {
                for (int i = 0; i < _spawnSettings.Count; i++)
                {
                    var asteroid = _spawner.Spawn() as Asteroid;
                    if (asteroid != null)
                    {
                        asteroid.Setup(AsteroidsConfiguration.Instance.GetAsteroidSettings(asteroid.AsteroidType));
                        asteroid.transform.position = transform.position;
                        asteroid.transform.SetParent(transform.parent);
                    }
                }
            }
        }
    }
}