using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalIndicator : MonoBehaviour
{
    Collider terrainCollider;
    void Awake()
    {
        terrainCollider = Terrain.activeTerrain.GetComponent<Collider>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftAlt)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (terrainCollider.Raycast(ray, out var hit, float.MaxValue))
            {
                transform.position = hit.point;
            }
        }
    }
}
