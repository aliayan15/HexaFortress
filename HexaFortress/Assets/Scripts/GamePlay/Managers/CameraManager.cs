using Managers;
using MyUtilities;
using System.Collections;
using System.Collections.Generic;
using Players;
using UnityEngine;


public class CameraManager : SingletonMono<CameraManager>
{
    public Camera MainCam => cam;
    public Transform CamPosition => myCamera;

    [SerializeField] private Camera cam;
    [SerializeField] private Transform myCamera;
    [SerializeField] private Transform globalLight;
    [Space(5)]
    [Header("Settings")]
    [SerializeField] private float normalSpeed;
    [SerializeField] private float fastSpeed;
    [Space(5)]
    [SerializeField] private float movementTime;
    [SerializeField] private float rotationAmount;
    [SerializeField] private Vector3 zoomAmount;
    [Space(5)]
    [SerializeField] private Vector2 lowBorder;
    [SerializeField] private Vector2 highBorder;

    private Vector3 _newPosition;
    private float _movementSpeed;
    private Quaternion _newRotation;
    private Vector3 _newZoom;


    private void Start()
    {
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
    }

    private void LateUpdate()
    {
        HandleInput();
    }

    private void HandleInput()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _newPosition += (transform.forward * _movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _newPosition += (transform.forward * -_movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _newPosition += (transform.right * _movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _newPosition += (transform.right * -_movementSpeed);
        }

        //if (Input.GetKey(KeyCode.Q))
        //{
        //    _newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        //}
        //if (Input.GetKey(KeyCode.E))
        //{
        //    _newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        //}

        if (Input.mouseScrollDelta.y > 0 && !Player.Instance.IsBuilding)
        {
            _newZoom += zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0 && !Player.Instance.IsBuilding)
        {
            _newZoom -= zoomAmount;
        }

        _newPosition.x = Mathf.Max(lowBorder.x, _newPosition.x);
        _newPosition.x = Mathf.Min(highBorder.x, _newPosition.x);
        _newPosition.z = Mathf.Max(lowBorder.y, _newPosition.z);
        _newPosition.z = Mathf.Min(highBorder.y, _newPosition.z);

        transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * movementTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * movementTime);
        myCamera.transform.localPosition = Vector3.Lerp(myCamera.transform.localPosition, _newZoom, Time.deltaTime * movementTime);
    }

    public void TeleportPosition(Vector3 pos)
    {
        transform.position = pos;
        _newPosition = transform.position;
    }
}

