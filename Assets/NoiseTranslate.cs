using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTranslate : MonoBehaviour
{
    float noise = 0.05f;
    float delay = 0.1f;
    IEnumerator Start()
    {
        var originPos = transform.position;
        while (true)
        {
            var newPos = new Vector3(CalcNoise(originPos.x)
                                    , CalcNoise(originPos.y)
                                    , CalcNoise(originPos.z));
            transform.DOMove(newPos, delay).SetLink(gameObject);
            yield return new WaitForSeconds(delay);
        }
    }
    float CalcNoise(float value)
    {
        return value + Random.Range(-noise, noise);
    }
}
