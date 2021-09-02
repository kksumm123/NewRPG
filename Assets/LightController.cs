using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    float minScale = 0.8f;
    float maxScale = 1.2f;
    float duration = 0.2f;

    IEnumerator Start()
    {
        Light light = GetComponent<Light>();
        float originRange = light.range;
        while (true)
        {
            var tween = DOTween.To(() => originRange, x => light.range = x
                            , originRange * Random.Range(minScale, maxScale)
                            , Random.Range(0, duration))
                   .SetLink(gameObject);
            yield return new WaitForSeconds(tween.Duration());
        }
    }
}

