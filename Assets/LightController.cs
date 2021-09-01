using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    float duration = 1f;

    void Start()
    {
        Light light = GetComponent<Light>();
        DOTween.To(() => light.intensity, x => light.intensity = x, 0, duration)
               .SetDelay(3)
               .OnComplete(() => light.enabled = false)
               .SetLink(gameObject);
    }
}
