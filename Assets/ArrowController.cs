using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour, IProjectile
{
    [SerializeField] GameObject arrowDecal = null;
    public float speed = 10f;
    [SerializeField] float timeToDestroy = 7f;

    public Vector3 Target { get => target; set => target = value; }
    public bool Hit { get => hit; set => hit = value; }
    public Vector3 TargetContactNormal { get => targetContactNormal; set => targetContactNormal = value; }
    [HideInInspector] Vector3 target;
    [HideInInspector] bool hit;
    [HideInInspector] Vector3 targetContactNormal;

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        //transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        //Vector3 forward = 
        if (!hit && Vector3.Distance(transform.position, target) < 0.01f)
        {
            if (hit)
            {
                Instantiate(arrowDecal, target, transform.rotation);
                isHit = true;
            }

            Destroy(gameObject);
        }
    }

    bool isHit = false;
    private void OnCollisionEnter(Collision other)
    {
        if (isHit == false)
        {
            isHit = true;
            var contact = other.GetContact(0);
            Instantiate(arrowDecal, contact.point, transform.rotation);
            Destroy(gameObject);
        }
    }
}
