using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxMaterialController : MonoBehaviour
{
    [SerializeField] Material skyboxMaterial;
    [SerializeField] float speed = 0.01f;
    [SerializeField] float currentRotation;
    float originRotation;
    bool isRotation = false;
    IEnumerator Start()
    {
        originRotation = skyboxMaterial.GetFloat("_Rotation");
        isRotation = true;
        while (isRotation)
        {
            currentRotation += speed * Time.deltaTime;
            skyboxMaterial.SetFloat("_Rotation", currentRotation);
            yield return null;
        }
    }
    private void OnDestroy()
    {
        skyboxMaterial.SetFloat("_Rotation", originRotation);
    }
}
