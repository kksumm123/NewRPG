using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    Animator animator;

    [SerializeField] float speed = 5;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Move();
    }

    #region Move
    Vector3 move;
    void Move()
    {
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move.z = 1;
        if (Input.GetKey(KeyCode.S)) move.z = -1;
        if (Input.GetKey(KeyCode.A)) move.x = -1;
        if (Input.GetKey(KeyCode.D)) move.x = 1;

        if (move != Vector3.zero)
        {
            Vector3 relateMove = Vector3.zero;
            // forward * move.z �Ҵ�
            relateMove = Camera.main.transform.forward * move.z;
            // right * move.x �Ҵ� (+= ���ִ� ������ forward �� ���� �����ֱ� ����)
            relateMove += Camera.main.transform.right * move.x;
            relateMove.y = 0;
            move = relateMove;

            move.Normalize();

            float forwardDegree = transform.forward.VectorToDegree();
            float moveDegree = move.VectorToDegree();
            float dirRadian = (moveDegree - forwardDegree + 90) * Mathf.PI / 180; //���Ȱ�
            Vector3 dir;
            dir.x = Mathf.Cos(dirRadian);// 
            dir.z = Mathf.Sin(dirRadian);//

            animator.SetFloat("DirX", dir.x);
            animator.SetFloat("DirY", dir.z);

            transform.Translate(speed * Time.deltaTime * move, Space.World);
        }

        animator.SetFloat("Speed", move.sqrMagnitude);
    }
    #endregion Move
}
public static class MyExtention
{
    public static float VectorToDegree(this Vector3 v)
    {
        float radian = Mathf.Atan2(v.z, v.x);
        return (radian * Mathf.Rad2Deg);
    }
}
