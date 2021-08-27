using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarder : MonoBehaviour
{
    public bool yOnly;
    Transform cameraTr;
    void Start()
    {
        cameraTr = Camera.main.transform;
        Billboarding();
    }

    void Update()
    {
        Billboarding();
    }

    private void Billboarding()
    {
        if (yOnly)
        {
            transform.rotation = cameraTr.rotation;
            var rotation = transform.rotation.eulerAngles;
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = Quaternion.Euler(rotation);
        }
        else
            transform.rotation = cameraTr.rotation;
    }
}
