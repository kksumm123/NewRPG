using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalIndicator : MonoBehaviour
{
    Collider terrainCollider;
    public GameObject indicatePrefab;
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
                //transform.position = hit.point;
                Instantiate(indicatePrefab
                            , hit.point + new Vector3(0, 0.5f, 0)
                            , Quaternion.identity);
            }
        }
    }
}
