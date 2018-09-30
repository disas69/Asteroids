using Framework.Attributes;
using Framework.Tools.Singleton;
using UnityEngine;

namespace Game.Configuration
{
    [ResourcePath("GameConfiguration")]
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "Configuration/GameConfiguration")]
    public class GameConfiguration : ScriptableSingleton<GameConfiguration>
    {
        public int LivesCount = 3;
        public float DelayBetweenSessions = 2f;
        public float AsteroidsSpawnDelay = 1.5f;
        public int MaxAsteroidsCount = 10;
    }
}