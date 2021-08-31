using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalIndicator : MonoBehaviour
{
    Collider terrainCollider;
    public GameObject indicatePrefab;
    LayerMask indicatorLayer;

    void Awake()
    {
        if (terrainCollider != null)
        {
            terrainCollider = Terrain.activeTerrain.GetComponent<Collider>();
            indicatorLayer = 1 << LayerMask.NameToLayer("Indicator");
        }
    }
    RaycastHit hit;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftAlt))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, float.MaxValue, indicatorLayer))
            {
                Destroy(hit.transform.gameObject);
            }
            else if (terrainCollider.Raycast(ray, out hit, float.MaxValue))
            {
                //transform.position = hit.point;
                Instantiate(indicatePrefab
                            , hit.point + new Vector3(0, 0.5f, 0)
                            , Quaternion.identity);
            }
        }
    }
}
