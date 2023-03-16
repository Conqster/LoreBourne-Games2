using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


namespace LoreBourne
{
    public class ShootingController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera aimingCamera;
        [SerializeField] private bool aim;


        private void Update()
        {


            GetInputs();   
            if (aim)
            {
                aimingCamera.gameObject.SetActive(true);
            }
            else
            {
                aimingCamera.gameObject.SetActive(false);   
            }
        }


        private void GetInputs()
        {
            aim = Input.GetButton("Aim");
        }
    }

}

