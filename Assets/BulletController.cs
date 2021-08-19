using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IProjectile
{
    public Vector3 Target { get; set; }
    public bool Hit { get; set; }
    public Vector3 TargetContactNormal { get; set; }
    public float CurrentAngle { get; set; }
}
public class BulletController : MonoBehaviour, IProjectile
{
    [SerializeField] GameObject bulletDecal = null;
    public float speed = 50f;
    [SerializeField] float timeToDestroy = 7f;

    public Vector3 Target { get => target; set => target = value; }
    public bool Hit { get => hit; set => hit = value; }
    public Vector3 TargetContactNormal { get => targetContactNormal; set => targetContactNormal = value; }
    public float CurrentAngle { get => 0; set { } }
    Vector3 target;
    bool hit;
    Vector3 targetContactNormal;
    float currentAngle;

    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        //transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        if (!hit && Vector3.Distance(transform.position, target) < 0.01f)
        {
            if (hit)
            {
                Instantiate(bulletDecal, target + targetContactNormal * 0.0001f, Quaternion.LookRotation(targetContactNormal));
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
            Instantiate(bulletDecal, contact.point + contact.normal * 0.0001f, Quaternion.LookRotation(contact.normal));
            Destroy(gameObject);
        }
    }
}