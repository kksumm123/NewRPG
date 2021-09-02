using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawImageAnimator : MonoBehaviour
{
    [SerializeField] float waitSecond = 0.1f;
    float xCount = 4;
    float yCount = 5;
    IEnumerator Start()
    {
        RawImage rawImage = GetComponent<RawImage>();
        var rect = rawImage.uvRect;
        
        float xValue = 1 / xCount; // 0.25
        float yValue = 1 / yCount; // 0.2

        while (true)
        {
            for (int y = 0; y < yCount; y++)
            {
                rect.y = 1 - ((y + 1) * yValue) + (y * 0.0005f);
                for (int x = 0; x < xCount; x++)
                {
                    rect.x = x * xValue;
                    print($"rect : {rect}");
                    rawImage.uvRect = rect;
                    yield return new WaitForSeconds(waitSecond);
                }
            }
        }
    }
}