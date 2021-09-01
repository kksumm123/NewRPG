using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArrowController : MonoBehaviour, IProjectile
{
    Rigidbody rigid;

    [SerializeField] GameObject arrowDecal = null;
    public float speed = 10f;
    [SerializeField] float timeToDestroy = 7f;
    [SerializeField] float torqueValue = 100;

    public Vector3 Target { get => target; set => target = value; }
    public bool Hit { get => hit; set => hit = value; }
    public Vector3 TargetContactNormal { get => targetContactNormal; set => targetContactNormal = value; }
    public float CurrentAngle { get => currentAngle; set => currentAngle = value; }
    public float Speed { get => speed; set => speed = value; }
    Vector3 target;
    bool hit;
    Vector3 targetContactNormal;
    float currentAngle;


    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
        rigid = GetComponent<Rigidbody>();

        // 이전 화살 날리는 코드
        //Vector3 toDirec = (target - transform.position).normalized;
        //rigid.AddForce(toDirec * speed, ForceMode.VelocityChange);

        // 궤적을 따라 날리는 코드
        Vector3 direction = target - transform.position;
        float yOffset = direction.y;
        direction = ProjectileMath.ProjectVectorOnPlane(Vector3.up, direction);
        float distance = direction.magnitude;

        bool targetInRange = ProjectileMath.LaunchAngle(speed, distance, yOffset
            , Physics.gravity.magnitude, out float angle0, out float angle1);
        if (targetInRange)
            currentAngle = angle1;

        transform.forward = direction;
        float degree = -currentAngle * Mathf.Rad2Deg;
        transform.Rotate(degree, 0, degree);
        rigid.velocity = rigid.transform.forward * speed;

        torqueRotate = transform.rotation.eulerAngles;
        DOTween.To(() => torqueRotate.z, value => torqueRotate.z = value
                    , 360, 0.1f).SetLoops(-1, LoopType.Restart).SetLink(gameObject);
    }
    Vector3 torqueRotate;
    private void Update()
    {
        if (rigid.velocity != Vector3.zero)
            transform.forward = rigid.velocity.normalized;


        //rigid.AddTorque(new Vector3(0, 0, 1) * torqueValue, ForceMode.Force);
        var rotation = transform.rotation.eulerAngles;
        rotation.z = torqueRotate.z;
        transform.rotation = Quaternion.Euler(rotation);

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
            var decalRotation = transform.rotation.eulerAngles
                            + new Vector3(0, 0, Random.Range(-90, 90));
            Instantiate(arrowDecal, contact.point, Quaternion.Euler(decalRotation));
            print($"{transform.position}, {transform.rotation.eulerAngles}\n{transform.rotation.eulerAngles} -> {decalRotation}");
            Destroy(gameObject);
        }
    }
}