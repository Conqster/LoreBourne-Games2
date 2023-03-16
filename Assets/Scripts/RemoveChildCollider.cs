using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveChildCollider : MonoBehaviour
{
    private GameObject _object;
    List<Transform> childrenTransform;

    // Start is called before the first frame update
    void Start()
    {
        _object = this.gameObject;

        foreach(Transform child in _object.transform)
        {
            Rigidbody childRb = child.gameObject.GetComponent<Rigidbody>();
            GameObject childObject = child.gameObject;
            //childRb
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
