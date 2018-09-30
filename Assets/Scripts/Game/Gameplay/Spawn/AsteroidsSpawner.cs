using System;
using System.Collections;
using System.Collections.Generic;
using Game.Configuration;
using Game.Gameplay.SpaceObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Gameplay.Spawn
{
    [Serializable]
    public class SpawnSetup
    {
        public Spawner Spawner;
        [Range(0f, 1f)] 
        public float Probability;
    }

    public class AsteroidsSpawner : MonoBehaviour
    {
        private Coroutine _spawnCoroutine;

        public List<SpawnSetup> SpawnSetups;
        public bool Spawning { get; private set; }

        private void Awake()
        {
            SpawnSetups.Sort((setup1, setup2) => setup1.Probability.CompareTo(setup2.Probability));
        }

        public void StartSpawn()
        {
            _spawnCoroutine = StartCoroutine(Spawn());
            Spawning = true;
        }

        public void RestartSpawn()
        {
            StopSpawn();
            StartSpawn();
        }

        private void StopSpawn()
        {
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
            }

            for (var i = 0; i < SpawnSetups.Count; i++)
            {
                SpawnSetups[i].Spawner.Flush();
            }

            var innerAsteroids = GetComponentsInChildren<Asteroid>();
            for (int i = 0; i < innerAsteroids.Length; i++)
            {
                innerAsteroids[i].Deactivate(false);
            }
            
            Spawning = false;
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(GameConfiguration.Instance.AsteroidsSpawnDelay);

                if (GetActiveAsteroidsCount() < GameConfiguration.Instance.MaxAsteroidsCount)
                {
                    var spawner = GetNextSpawner(Random.value);
                    if (spawner != null)
                    {
                        var spaceAsteroid = spawner.Spawn() as Asteroid;
                        if (spaceAsteroid != null)
                        {
                            spaceAsteroid.Setup(AsteroidsConfiguration.Instance.GetAsteroidSettings(spaceAsteroid.AsteroidType));
                        }
                    }
                }
            }
        }

        private Spawner GetNextSpawner(float chance)
        {
            for (int i = 0; i < SpawnSetups.Count; i++)
            {
                var spawnSetup = SpawnSetups[i];
                if (spawnSetup.Probability >= chance)
                {
                    return spawnSetup.Spawner;
                }
            }

            return null;
        }

        private int GetActiveAsteroidsCount()
        {
            var count = 0;
            for (var i = 0; i < SpawnSetups.Count; i++)
            {
                count += SpawnSetups[i].Spawner.Count;
            }

            return count;
        }
    }
}