using System;
using System.Collections.Generic;
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
        private List<SpaceObject> _activeObjects;

        [SerializeField] private bool _activateOnAwake = true;
        [SerializeField] private SpawnSettings _spawnSettings;
        
        public int Count
        {
            get { return _activeObjects.Count; }
        }

        private void Awake()
        {
            _activeObjects = new List<SpaceObject>();

            if (_activateOnAwake)
            {
                Activate(_spawnSettings);
            }
        }

        public void Activate(SpawnSettings spawnSettings)
        {
            if (_objectsPool == null)
            {
                _objectsPool = new Pool<SpaceObject>(spawnSettings.ObjectPrefab, transform, spawnSettings.PoolCapacity);
            }
        }

        public SpaceObject Spawn()
        {
            var spaceObject = _objectsPool.GetNext();
            spaceObject.Deactivated += OnSpaceObjectDeactivated;
            _activeObjects.Add(spaceObject);
            
            return spaceObject;
        }

        public void Flush()
        {
            for (int i = 0; i < _activeObjects.Count; i++)
            {
                Despawn(_activeObjects[i]);
            }
        }

        private void OnSpaceObjectDeactivated(SpaceObject spaceObject)
        {
            Despawn(spaceObject);
        }

        private void Despawn(SpaceObject spaceObject)
        {
            spaceObject.Deactivated -= OnSpaceObjectDeactivated;

            _activeObjects.Remove(spaceObject);
            _objectsPool.Return(spaceObject);
        }
    }
}