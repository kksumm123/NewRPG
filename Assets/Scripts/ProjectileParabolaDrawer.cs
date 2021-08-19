using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileParabolaDrawer : MonoBehaviour
{
    [SerializeField] ProjectileParabola projectileArc;
    [SerializeField] Transform firePoint;
    [SerializeField] LayerMask mapLayer;
    [SerializeField] float bulletHitMissDistance = 25f;

    public float speed = 20;
    void Start()
    {
        projectileArc = GetComponent<ProjectileParabola>();
        firePoint = transform;
        //mapLayer = 1 << LayerMask.NameToLayer("Environment");
        mapLayer = int.MaxValue;
    }
    Vector3 targetPoint;
    void Update()
    {
        Vector3 rayStartPoint = Camera.main.transform.position;
        Vector3 cameraPositionSameY = Camera.main.transform.position;
        cameraPositionSameY.y = transform.position.y;
        float playerDistance = Vector3.Distance(cameraPositionSameY, transform.position);
        rayStartPoint += Camera.main.transform.forward * playerDistance;

        bool isHit = Physics.Raycast(rayStartPoint
                , Camera.main.transform.forward
                , out RaycastHit hit, Mathf.Infinity, mapLayer);
        if (isHit)
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = Camera.main.transform.position
                    + Camera.main.transform.forward * bulletHitMissDistance;
        }
        SetTargetWithSpeed(targetPoint, speed);
    }

    public float currentAngle;
    Vector3 direction;
    public void SetTargetWithSpeed(Vector3 point, float speed)
    {
        direction = point - firePoint.position;
        float yOffset = direction.y;
        direction = ProjectileMath.ProjectVectorOnPlane(Vector3.up, direction);
        float distance = direction.magnitude;
        float angle0, angle1;
        bool targetInRange = ProjectileMath.LaunchAngle(speed, distance, yOffset, Physics.gravity.magnitude, out angle0, out angle1);
        if (targetInRange)
            currentAngle = angle1;
        projectileArc.UpdateArc(speed, distance, Physics.gravity.magnitude, currentAngle, direction, targetInRange);
    }
}