using System;
using System.Collections;
using System.Collections.Generic;
using Game.Configuration;
using Game.Gameplay.SpaceObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Gameplay.Spawn
{
    public class AsteroidsSpawner : MonoBehaviour
    {
        private Coroutine _spawnCoroutine;
        private Dictionary<SpaceObject, Spawner> _activeSpaceObjects;

        [SerializeField] private List<SpawnSetup> _spawnSetups;
        [SerializeField] private float _spawnDelay;
        [SerializeField] private int _maxAsteroidsCount;

        private void Awake()
        {
            _spawnSetups.Sort((setup1, setup2) => setup1.Probability.CompareTo(setup2.Probability));
            _activeSpaceObjects = new Dictionary<SpaceObject, Spawner>();

            _spawnCoroutine = StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnDelay);

                if (_activeSpaceObjects.Count < _maxAsteroidsCount)
                {
                    var spawner = GetNextSpawner(Random.value);
                    if (spawner != null)
                    {
                        var spaceAsteroid = spawner.Spawn() as Asteroid;
                        if (spaceAsteroid != null)
                        {
                            spaceAsteroid.Deactivated += OnAsteroidDeactivated;
                            spaceAsteroid.Push(
                                AsteroidsConfiguration.Instance.GetAsteroidSettings(spaceAsteroid.AsteroidType));

                            _activeSpaceObjects.Add(spaceAsteroid, spawner);
                        }
                    }
                }
            }
        }

        private void OnAsteroidDeactivated(SpaceObject spaceAsteroid)
        {
            spaceAsteroid.Deactivated -= OnAsteroidDeactivated;

            Spawner spawner;
            if (_activeSpaceObjects.TryGetValue(spaceAsteroid, out spawner))
            {
                spawner.Despawn(spaceAsteroid);
            }

            _activeSpaceObjects.Remove(spaceAsteroid);
        }

        private Spawner GetNextSpawner(float chance)
        {
            for (int i = 0; i < _spawnSetups.Count; i++)
            {
                var spawnSetup = _spawnSetups[i];
                if (spawnSetup.Probability >= chance)
                {
                    return spawnSetup.Spawner;
                }
            }

            return null;
        }
    }

    [Serializable]
    public class SpawnSetup
    {
        public Spawner Spawner;
        [Range(0f, 1f)] public float Probability;
    }
}