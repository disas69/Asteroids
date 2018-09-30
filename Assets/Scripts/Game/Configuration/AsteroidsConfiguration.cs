using System.Collections.Generic;
using Framework.Attributes;
using Framework.Tools.Singleton;
using Game.Gameplay.SpaceObjects;
using UnityEngine;

namespace Game.Configuration
{
    [ResourcePath("AsteroidsConfiguration")]
    [CreateAssetMenu(fileName = "AsteroidsConfiguration", menuName = "Configuration/AsteroidsConfiguration")]
    public class AsteroidsConfiguration : ScriptableSingleton<AsteroidsConfiguration>
    {
        public List<AsteroidSettings> AsteroidsSettings = new List<AsteroidSettings>();

        public AsteroidSettings GetAsteroidSettings(AsteroidType type)
        {
            var settings = AsteroidsSettings.Find(s => s.Type == type);
            if (settings != null)
            {
                return settings;
            }

            Debug.LogError(string.Format("Failed to find AsteroidSettings of type: {0}", type));
            return null;
        }
    }
}