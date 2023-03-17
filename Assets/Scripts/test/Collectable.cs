using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoreBourne
{
    public class Collectable : MonoBehaviour
    {
        //ref to player inventory 
        //ref to manager 
        [SerializeField] protected Transform target;
        [SerializeField] protected int value;
        [SerializeField, Range(0, 10)] protected float timeToSelfDestory;

        [Space][Space]
        [Header("Collectable Behaviour")]
        [SerializeField, Range(0f, 20f)] protected float amplitude = 3f;
        protected float oscillator;
        protected float X;
        protected float hDir = -1f;
        [SerializeField, Range(0f, 4f)] protected float period = 1.05f;
        [SerializeField, Range(0f, 4f)] protected float spinRate = 1.05f;
        protected float phase = -0.5f;
        protected float vShift = 0f;

        protected virtual void Start()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == target.transform)
            {
                GiveItem();
            }
        }


        protected virtual void GiveItem()
        {
            Invoke("Disable", timeToSelfDestory);
        }

        protected virtual void DisableMe()
        {
            Destroy(gameObject);
        }
        private void Update()
        {
            CalculateOscillation();
            OscillateVertically();
            SpinCollectable();
        }


        void CalculateOscillation()
        {
            X += Time.deltaTime;
            oscillator = amplitude * Mathf.Sin(((2 * Mathf.PI) / period) * (X + phase)) + vShift;
        }

        private void OscillateVertically()
        {
            transform.Translate(0, oscillator * Time.deltaTime, 0, Space.World);
        }
        private void SpinCollectable()
        {
            transform.RotateAround(transform.position, Vector3.up, spinRate);
        }

    }
}


