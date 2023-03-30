using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapSystem : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 newPos = objectToFollow.position;
        newPos.y = transform.position.y;

        transform.position = newPos;
    }
}
