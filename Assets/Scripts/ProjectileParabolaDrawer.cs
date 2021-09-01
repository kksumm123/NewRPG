using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileParabolaDrawer : MonoBehaviour
{
    [SerializeField] ProjectileParabola projectileArc;
    [SerializeField] Transform firePoint;
    [SerializeField] LayerMask mapLayer;
    [SerializeField] float bulletHitMissDistance = 25f;

    public float Speed
    {
        get => speed;
        set
        {
            if (speed == value)
                return;
            Debug.Log($"{speed} => {value}");

            speed = value;
        }
    }
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
        Vector3 rayStartPoint = GetRayStartPointFromPlayer(Camera.main.transform, transform.root);

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
        SetTargetWithSpeed(targetPoint, Speed);
    }

    public static Vector3 GetRayStartPointFromPlayer(Transform cameraTransform, Transform playerTransform)
    {
        Vector3 rayStartPoint = cameraTransform.position;
        Vector3 cameraPositionSameY = cameraTransform.position;
        cameraPositionSameY.y = playerTransform.position.y;
        float playerDistance = Vector3.Distance(cameraPositionSameY, playerTransform.position);
        rayStartPoint += cameraTransform.forward * playerDistance;
        return rayStartPoint;
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
        else
            currentAngle = 1;

        projectileArc.UpdateArc(speed, distance, Physics.gravity.magnitude, currentAngle, direction, targetInRange);
    }
}