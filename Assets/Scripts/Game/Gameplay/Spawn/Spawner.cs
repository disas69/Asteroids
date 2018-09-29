using System;
using Framework.Tools.Gameplay;
using Game.Configuration;
using Game.Gameplay.SpaceObjects;
using UnityEngine;

namespace Game.Gameplay.Spawn
{
    [Serializable]
    public class Spawner : MonoBehaviour
    {
        private Pool<SpaceObject> _objectsPool;

        [SerializeField] private bool _activateOnAwake = true;
        [SerializeField] private SpawnSettings _spawnSettings;

        private void Awake()
        {
            if (_activateOnAwake)
            {
                Activate(_spawnSettings);
            }
        }

        public void Activate(SpawnSettings spawnSettings)
        {
            if (_objectsPool != null)
            {
                Destroy(_objectsPool.PoolRoot.gameObject);
            }
            
            _objectsPool = new Pool<SpaceObject>(spawnSettings.ObjectPrefab, transform, spawnSettings.PoolCapacity);
        }

        public SpaceObject Spawn()
        {
            return _objectsPool.GetNext();
        }

        public void Despawn(SpaceObject spaceObject)
        {
            _objectsPool.Return(spaceObject);
        }
    }
}