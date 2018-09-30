using System;
using Game.Gameplay.SpaceObjects;

namespace Game.Gameplay.Spawn
{
    [Serializable]
    public class SpawnSettings
    {
        public SpaceObject ObjectPrefab;
        public int Count;
        public int PoolCapacity;
    }
}