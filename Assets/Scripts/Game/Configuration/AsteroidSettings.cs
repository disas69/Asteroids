using System;
using Game.Gameplay.SpaceObjects;
using Game.Gameplay.Spawn;

namespace Game.Configuration
{
    [Serializable]
    public class AsteroidSettings
    {
        public AsteroidType Type;
        public float InitialVelocity;
        public float MaxVelocity;
        public float MaxTorque;
        public bool SpawnOnDestroy;
        public int ScorePoints;
        public SpawnSettings SpawnOnDestroySettings = new SpawnSettings();
    }
}