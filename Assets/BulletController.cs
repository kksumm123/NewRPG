using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] GameObject bulletDecal = null;
    public float speed = 50f;
    float timeToDestroy = 3f;
    public Vector3 target;
    public bool hit;
    public Vector3 targetContactNormal;

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }
    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        if (!hit && Vector3.Distance(transform.position, target) < 1f)
        {
            if (hit)
            {
                Instantiate(bulletDecal, target + targetContactNormal * 0.0001f, Quaternion.LookRotation(targetContactNormal));
                isHit = true;
            }

            Destroy(gameObject);
            print("Update");
        }
    }

    bool isHit = false;
    private void OnCollisionEnter(Collision other)
    {
        if (isHit == false)
        {
            isHit = true;
            var contact = other.GetContact(0);
            Instantiate(bulletDecal, contact.point + contact.normal * 0.0001f, Quaternion.LookRotation(contact.normal));
            Destroy(gameObject);
            print("ColEnter");
        }
    }
}