﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction shootAction;
    Animator animator;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField] float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    [SerializeField] float rotationSpeed = 15;

    void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        shootAction.performed += ShootAction_performed;
    }

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject barrelTransform;
    [SerializeField] Transform bulletParent;
    [SerializeField] float bulletHitMissDistance = 25f;
    void ShootAction_performed(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("FireStart");
        GameObject bullet = Instantiate(bulletPrefab, barrelTransform.transform.position, Camera.main.transform.rotation, bulletParent);
        var bulletController = bullet.GetComponent<BulletController>();

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = Camera.main.transform.position + Camera.main.transform.forward * bulletHitMissDistance;
            bulletController.hit = true;
        }
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        var input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * Camera.main.transform.right + move.z * Camera.main.transform.forward;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move.sqrMagnitude > 0)
        {
            float forwardDegree = transform.forward.VectorToDegree();
            float moveDegree = move.VectorToDegree();
            float dirRadian = (moveDegree - forwardDegree + 90) * Mathf.PI / 180; //라디안값
            Vector3 dir;
            dir.x = Mathf.Cos(dirRadian);// 
            dir.z = Mathf.Sin(dirRadian);//

            animator.SetFloat("DirX", dir.x);
            animator.SetFloat("DirY", dir.z);
        }
        animator.SetFloat("Speed", move.sqrMagnitude);

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        Quaternion targetRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}