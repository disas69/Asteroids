using Framework.Attributes;
using Framework.Tools.Singleton;
using UnityEngine;

namespace Game.Configuration
{
    [ResourcePath("GameConfiguration")]
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "Configuration/GameConfiguration")]
    public class GameConfiguration : ScriptableSingleton<GameConfiguration>
    {
        public float BallKickOffDelay;
        public float RacketMaxSize;
        public float RacketMinSize;
        public float RacketSizeDecreaseValue;
        public int RacketSizeDecreaseRate;
    }
}