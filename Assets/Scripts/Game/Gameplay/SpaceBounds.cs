using Framework.Tools.Singleton;
using Game.Camera;
using UnityEngine;

namespace Game.Gameplay
{
    public class SpaceBounds : MonoSingleton<SpaceBounds>
    {
        [SerializeField] private CameraController _camera;
        [SerializeField] private BoxCollider _leftBound;
        [SerializeField] private BoxCollider _rightBound;
        [SerializeField] private BoxCollider _topBound;
        [SerializeField] private BoxCollider _bottomBound;
        [SerializeField] private bool _executeInUpdate;

        public Vector2 LeftBound
        {
            get { return _leftBound.transform.position; }
        }

        public Vector2 RightBound
        {
            get { return _rightBound.transform.position; }
        }

        public Vector2 TopBound
        {
            get { return _topBound.transform.position; }
        }

        public Vector2 BottomBound
        {
            get { return _bottomBound.transform.position; }
        }

        protected override void Awake()
        {
            base.Awake();
            Calculate();
        }

        private void Update()
        {
            if (_executeInUpdate)
            {
                Calculate();
            }
        }

        private void Calculate()
        {
            var bounds = _camera.Bounds;
            CalculateBoundsSize(bounds);
            CalculateBoundsPositions(bounds);
        }

        private void CalculateBoundsSize(Bounds bounds)
        {
            var hight = bounds.extents.y * 2f;
            _leftBound.size = new Vector3(_leftBound.size.x, hight, _leftBound.size.z);
            _rightBound.size = new Vector3(_rightBound.size.x, hight, _rightBound.size.z);

            var width = bounds.extents.x * 2f;
            _topBound.size = new Vector3(width, _topBound.size.y, _topBound.size.z);
            _bottomBound.size = new Vector3(width, _bottomBound.size.y, _bottomBound.size.z);
        }

        private void CalculateBoundsPositions(Bounds bounds)
        {
            var leftPosition = new Vector3((bounds.center.x - bounds.extents.x) - _leftBound.size.x / 2f, 0f, 0f);
            _leftBound.transform.position = leftPosition;

            var rightPosition = new Vector3((bounds.center.x + bounds.extents.x) + _rightBound.size.x / 2f, 0f, 0f);
            _rightBound.transform.position = rightPosition;

            var topPosition = new Vector3(0f, (bounds.center.y + bounds.extents.y) + _topBound.size.y / 2f, 0f);
            _topBound.transform.position = topPosition;

            var bottomPosition = new Vector3(0f, (bounds.center.y - bounds.extents.y) - _bottomBound.size.y / 2f, 0f);
            _bottomBound.transform.position = bottomPosition;
        }
    }
}