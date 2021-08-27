using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    float camY;
    void Start()
    {
        camY = transform.position.y;
    }
    void LateUpdate()
    {
        var pos = transform.position;
        pos = target.position;
        pos.y = camY;
        transform.position = pos;
    }
}
