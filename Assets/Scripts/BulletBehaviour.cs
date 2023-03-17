using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public GameObject target;

    [SerializeField] public Transform refDirection;
    private Rigidbody bulletRb;

    /// <summary>
    /// I might have like a ref to player/ weapon to check the direction the ref is facing 
    /// to set the forward of that of bullet so it could shoot toward that  direction
    /// </summary>

    private void Awake()
    {
        bulletRb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// this was added after bulletBehaviour was created for NPC
    /// in the NPC script, force is been applied when the bullet is been initiatiated
    /// 
    /// </summary>
    private void Start()
    {

        transform.forward = refDirection.forward;

        //float speed = 10f;
        //bulletRb.velocity = transform.forward * speed;
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Player")
        {
            print("trying to deal damge to player");
        }


        Destroy(gameObject);
    }


}
