using MyUtilities;
using UnityEngine;
using Random = UnityEngine.Random;


namespace HexaFortress.Game
{
    public class CameraManager : SingletonMono<CameraManager>
    {
        [SerializeField] private InputReader inputReader;
        public Camera MainCam => cam;
        public Transform CamPosition => myCamera;

        [SerializeField] private Camera cam;
        [SerializeField] private Transform myCamera;
        [SerializeField] private Transform globalLight;

        [Space(5)] [Header("Settings")] [SerializeField]
        private float normalSpeed;

        [SerializeField] private float fastSpeed;
        [Space(5)] [SerializeField] private float movementTime;
        [SerializeField] private float rotationAmount;
        [SerializeField] private Vector3 zoomAmount;
        [Space(5)] [SerializeField] private Vector2 lowBorder;
        [SerializeField] private Vector2 highBorder;

        private Vector3 _newPosition;
        private float _movementSpeed;
        private Quaternion _newRotation;
        private Vector3 _newZoom;
        private bool _isPlayerBuilding;
        private Vector2 _moveInput;

        private void Start()
        {
            EventManager.AddListener<OnPlayerBuildModeEvent>(OnPlayerBuildMode);
            int rndRosY = -Random.Range(20, 160);
            Quaternion ros = Quaternion.Euler(new Vector3(0, rndRosY, 0));
            transform.rotation = ros;
            Vector3 rosAngle = globalLight.rotation.eulerAngles;
            rosAngle.y = rndRosY;
            ros = Quaternion.Euler(rosAngle);
            globalLight.rotation = ros;

            _newPosition = transform.position;
            _newRotation = transform.rotation;
            _newZoom = myCamera.transform.localPosition;
            _movementSpeed = normalSpeed;
            inputReader.MovementEvent += OnMove;
            inputReader.ScrollEvent += OnScroll;
        }

        private void OnScroll(float scroll)
        {
            if (Input.mouseScrollDelta.y > 0 && !_isPlayerBuilding)
            {
                _newZoom += zoomAmount;
            }

            if (Input.mouseScrollDelta.y < 0 && !_isPlayerBuilding)
            {
                _newZoom -= zoomAmount;
            }
        }

        private void OnMove(Vector2 input)
        {
            _moveInput = input;
        }

        private void LateUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            _newPosition += transform.forward * (_movementSpeed * _moveInput.y);
            _newPosition += transform.right * (_movementSpeed * _moveInput.x);
           
            _newPosition.x = Mathf.Max(lowBorder.x, _newPosition.x);
            _newPosition.x = Mathf.Min(highBorder.x, _newPosition.x);
            _newPosition.z = Mathf.Max(lowBorder.y, _newPosition.z);
            _newPosition.z = Mathf.Min(highBorder.y, _newPosition.z);

            transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * movementTime);
            myCamera.transform.localPosition =
                Vector3.Lerp(myCamera.transform.localPosition, _newZoom, Time.deltaTime * movementTime);
        }

        public void TeleportPosition(Vector3 pos)
        {
            transform.position = pos;
            _newPosition = transform.position;
        }

        private void OnPlayerBuildMode(OnPlayerBuildModeEvent obj)
        {
            _isPlayerBuilding = obj.IsBuilding;
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<OnPlayerBuildModeEvent>(OnPlayerBuildMode);
            inputReader.MovementEvent -= OnMove;
            inputReader.ScrollEvent -= OnScroll;
        }
    }
}