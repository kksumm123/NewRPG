using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] string parameterAttack = "FireStart";
    [SerializeField] string parameterSpeed = "Speed";
    [SerializeField] string parameterIsMoving = "IsMoving";

    public PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction shootAction;
    InputAction aimAction;
    Animator animator;
    ProjectileParabolaDrawer projectileParabolaDrawer;

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
        projectileParabolaDrawer = GetComponentInChildren<ProjectileParabolaDrawer>();
        projectileParabolaDrawer.gameObject.SetActive(false);

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        aimAction = playerInput.actions["Aim"];
        shootAction.performed += ShootAction_performed;

        aimAction.performed += AimAction_performed;
        aimAction.canceled += AimAction_canceled;

        projectileParabolaDrawer.Speed = bulletPrefab.GetComponent<IProjectile>().Speed;
        mapLayer = int.MaxValue;
        mapLayer &= ~(1 << LayerMask.NameToLayer("Defalt"));
        mapLayer &= ~(1 << LayerMask.NameToLayer("Player"));
        mapLayer &= ~(1 << LayerMask.NameToLayer("NPC"));
    }

    void AimAction_performed(InputAction.CallbackContext obj)
    {
        projectileParabolaDrawer.gameObject.SetActive(true);
    }
    private void AimAction_canceled(InputAction.CallbackContext obj)
    {
        projectileParabolaDrawer.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        shootAction.performed -= ShootAction_performed;
        aimAction.performed -= AimAction_performed;
        aimAction.canceled -= AimAction_canceled;
    }

    #region ShootAction_performed
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject barrelTransform;
    [SerializeField] Transform bulletParent;
    [SerializeField] float bulletHitMissDistance = 25f;
    [SerializeField] LayerMask mapLayer;
    [SerializeField] GameObject targetPosGo;
    void ShootAction_performed(InputAction.CallbackContext obj)
    {
        if (StageManager.GameState != GameStateType.Play)
            return;

        animator.SetTrigger(parameterAttack);

        GameObject projectile = bulletPrefab;
        if (nextSkillProjectile != null)
        {
            projectile = nextSkillProjectile;
            nextSkillProjectile = null;
        }
        GameObject bullet = Instantiate(projectile, barrelTransform.transform.position
            , Quaternion.LookRotation(Camera.main.transform.forward), bulletParent);
        var bulletController = bullet.GetComponent<IProjectile>();
        bulletController.CurrentAngle = projectileParabolaDrawer.currentAngle;
        projectileParabolaDrawer.Speed = bulletController.Speed;
        Vector3 rayStartPoint = ProjectileParabolaDrawer.GetRayStartPointFromPlayer(Camera.main.transform, transform);
        bool isHit = Physics.Raycast(rayStartPoint, Camera.main.transform.forward, out RaycastHit hit, Mathf.Infinity, mapLayer);
        if (isHit)
        {
            bullet.transform.LookAt(hit.point);
            //StartCoroutine(PointBulletTargetsPos(bullet, hit.point));
            bulletController.Target = hit.point;
            bulletController.TargetContactNormal = hit.normal;
            bulletController.Hit = true;
        }
        else
        {
            bulletController.Target = Camera.main.transform.position + Camera.main.transform.forward * bulletHitMissDistance;
            bulletController.Hit = true;
        }
    }


    #endregion ShootAction_performed
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
        animator.SetFloat(parameterSpeed, move.sqrMagnitude);

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

    public GameObject nextSkillProjectile;
    internal void UseSkill(SkillInfo skillInfo)
    {
        // skillInfo.arrowPrefabName
        // = 다음 발사에 사용할 화살
        if (string.IsNullOrEmpty(skillInfo.arrowPrefabName) == false)
        {
            nextSkillProjectile = (GameObject)Resources.Load(skillInfo.arrowPrefabName);
        }
    }
}