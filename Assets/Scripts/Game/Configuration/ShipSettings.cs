using Framework.Attributes;
using Framework.Tools.Singleton;
using UnityEngine;

namespace Game.Configuration
{
    [ResourcePath("ShipSettings")]
    [CreateAssetMenu(fileName = "SpaceShipSettings", menuName = "Configuration/SpaceShipSettings")]
    public class ShipSettings : ScriptableSingleton<ShipSettings>
    {
        public float MovingForce = 15f;
        public float RotationSpeed = 200.0f;
        public float LinearDrag = 1.5f;
        public float AngularDrag = 1f;
        public float ShotDelay = 0.1f;
    }
}