using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFallScroller : MonoBehaviour
{
    new Renderer renderer;
    [SerializeField] float speed = 0.3f;
    Vector2 currentOffset;
    private bool isScroller = false;
    IEnumerator Start()
    {
        renderer = GetComponent<Renderer>();
        var matOffset = currentOffset = renderer.sharedMaterial.mainTextureOffset;
        isScroller = true;
        while (isScroller)
        {
            matOffset.y += speed * Time.deltaTime;
            renderer.sharedMaterial.mainTextureOffset = matOffset;
            yield return null;
        }
    }
    private void OnDestroy()
    {
        renderer.sharedMaterial.mainTextureOffset = currentOffset;
    }
}