using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne;
using Cinemachine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private float mouseInputX;
    [SerializeField] private float mouseInputY;

    [Header("Sensitivity")]
    [SerializeField, Range(0f, 100f)] private float mouseSensitivity;
    [SerializeField, Range(0f, 2f)] private float RightStickSensitivity;

    [Space][Space]
    [Header("Player's Input")]
    [SerializeField] private PlayerInputType playerInputSystem;
    [SerializeField] private bool playerIsAiming;

    [Space][Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera FollowCam;
    [SerializeField] private CinemachineVirtualCamera AimCam;
    [SerializeField, Range(-1, 1)] private int NormalToAim;

    [Space][Header("Set Priority Values")]
    [SerializeField, Range(0,10)] private int followCamPriority;
    [SerializeField, Range(0,10)] private int aimCamPriority;


    [Space][Header("New New New!!!!!")]
    [SerializeField] private Transform target;
    [SerializeField] bool useTimeDeltaTime;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;



    private void Start()
    {
        InitializePriorities();
    }

    private void Update()
    {
        Inputs();
        //CameraRotation();
        //InputsRaw();

        //print("Input on mouse X-axis" + mouseInputX + "Input on mouse Y-axis" + mouseInputY);
        //print("Input on mouse Y-axis" + mouseInputY);
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void InputsRaw()
    {
        float x = 0f;
        float y = 0f;

        switch (playerInputSystem)
        {
            case PlayerInputType.Controller:
                x = Input.GetAxisRaw("Right JoyStick X");
                y = Input.GetAxisRaw("Right JoyStick Y");
                break;
            case PlayerInputType.Keyboard:
                x = Input.GetAxisRaw("Mouse X");
                y = Input.GetAxisRaw("Mouse Y");
                break;
        }

        mouseInputX = x;
        mouseInputY = y;
    }

    private void Inputs()
    {
        float x = 0f;
        float y = 0f;

        switch(playerInputSystem)
        {
            case PlayerInputType.Controller:
                x = Input.GetAxis("Right JoyStick X");
                y = Input.GetAxis("Right JoyStick Y");
                break;
            case PlayerInputType.Keyboard:
                x = Input.GetAxis("Mouse X");
                y = Input.GetAxis("Mouse Y");
                break;
        }

        mouseInputX = x;
        mouseInputY = y;
        //mouseInputX = Mathf.Clamp(x, -1, 1);
        //mouseInputY = Mathf.Clamp(y, -1, 1);
    }

    private void CameraRotation()
    {

        //Don't multiply mouse input by Time.deltaTime;
        //float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
        float deltaTimeMultiplier = useTimeDeltaTime ? Time.deltaTime : 1.0f;

        //_cinemachineTargetYaw += mouseInputX * deltaTimeMultiplier;
        //_cinemachineTargetPitch += mouseInputY * deltaTimeMultiplier;

        _cinemachineTargetYaw += mouseInputX * mouseSensitivity;
        _cinemachineTargetPitch += mouseInputY * mouseSensitivity;

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void GetTarget()
    {

    }


    private void InitializePriorities()
    {
        if(FollowCam && AimCam)
        {
            FollowCam.Priority = followCamPriority;
            AimCam.Priority = aimCamPriority;
        }

    }


}
