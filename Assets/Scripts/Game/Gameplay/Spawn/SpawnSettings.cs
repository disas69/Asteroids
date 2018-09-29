using System;
using Game.Gameplay.SpaceObjects;
using UnityEngine;

namespace Game.Configuration
{
    [Serializable]
    public class SpawnSettings
    {
        public SpaceObject ObjectPrefab;
        public int Count;
        public int PoolCapacity;
    }
}