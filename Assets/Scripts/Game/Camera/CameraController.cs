using UnityEngine;

namespace Game.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private bool _updateBounds;

        public UnityEngine.Camera Camera { get; private set; }
        public Bounds Bounds { get; private set; }

        private void Awake()
        {
            Camera = GetComponent<UnityEngine.Camera>();
            Bounds = CalculateOrthographicBounds(Camera);
        }

        private void Update()
        {
            if (_updateBounds)
            {
                Bounds = CalculateOrthographicBounds(Camera);
            }
        }

        private static Bounds CalculateOrthographicBounds(UnityEngine.Camera camera)
        {
            var screenAspect = (float) Screen.width / (float) Screen.height;
            var cameraHeight = camera.orthographicSize * 2;
            return new Bounds(camera.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        }
    }
}