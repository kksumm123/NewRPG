using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    ParticleSystem ps;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Play();
    }

    void Update()
    {
        if (ps.isPlaying)
            return;

        // 파티클 더이상 안나온다면
        Destroy(gameObject);
    }
}
