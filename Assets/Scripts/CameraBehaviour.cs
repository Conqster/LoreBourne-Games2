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
    [SerializeField, Range(0f, 15f)] private float mouseSensitivity;
    [SerializeField, Range(0f, 15f)] private float rightStickSensitivity = 7.67f;
    [SerializeField, Range(0, 15f)] private float aimSensitivity = 7.67f;
    private float normalSensitity;

    [Space][Space]
    [Header("Player's Input")]
    [SerializeField] private PlayerInputType playerInputSystem;
    [SerializeField] private PlayerBehaviour player;
    [SerializeField] private bool playerIsAiming;

    [Space][Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera FollowCam;
    [SerializeField] private CinemachineVirtualCamera AimCam;
    [SerializeField, Range(-1, 1)] private int NormalToAim;

    [Space][Header("Set Priority Values")]
    [SerializeField, Range(0,10)] private int followCamPriority;
    [SerializeField, Range(0,10)] private int  aimCamPriority;
    [SerializeField, Range(0,10)] private int currentCamPriority;
    

    [Space][Header("New New New!!!!!")]
    [SerializeField] private Transform target;
    [SerializeField] bool useTimeDeltaTime;

    [Header("Camera Behaviour")]
    [SerializeField] private GameObject cameraFocus;
    [SerializeField, Range(50,100)] private float TopClamp = 70f;
    [SerializeField, Range(-45,-20)] private float BottomClamp = -30.0f;
    [SerializeField] private float CameraAngleOverride = 0.0f;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;



    private void Start()
    {
        InitializePriorities();
    }

    private void Update()
    {

        switch (playerInputSystem)
        {
            case PlayerInputType.Controller:
                normalSensitity = rightStickSensitivity;
                break;
            case PlayerInputType.Keyboard:
                normalSensitity = mouseSensitivity;
                break;
        }

        PrioritiesCamera(ref aimCamPriority, ref followCamPriority);
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

        playerIsAiming = player.Aiming();
        //mouseInputX = Mathf.Clamp(x, -1, 1);
        //mouseInputY = Mathf.Clamp(y, -1, 1);
    }


    private void PrioritiesCamera(ref int aim, ref int follow)
    {
        //AimCam.Priority = playerIsAiming ? currentCamPriority : aim;
        //FollowCam.Priority = !playerIsAiming ? currentCamPriority : follow;

        GameObject AimCamera = AimCam.transform.gameObject;
        bool activate = playerIsAiming ? true : false;
        AimCamera.SetActive(activate);

    }

    private void CameraRotation()
    {

        float sensitivity = playerIsAiming ? aimSensitivity : normalSensitity;

        _cinemachineTargetYaw += mouseInputX * sensitivity;
        _cinemachineTargetPitch += mouseInputY * sensitivity;

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        cameraFocus.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
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
