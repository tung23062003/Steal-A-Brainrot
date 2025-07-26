using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovable, IJumpable
{
    [Header("Move Settings")]
    public float jumpForce = 8.0f;
    public float speed = 2.0f;
    public float rotationSpeed = 15.0f;


    [Header("Ground Check")]
    public Transform groundCheckPoint;
    [SerializeField] protected LayerMask groundLayer;
    public Vector3 groundCheckVector;


    [SerializeField] protected LayerMask groundTopTowerLayer;

    public float currentSpeed;

    private Player playerController;
    [HideInInspector] public Rigidbody rb;
    private StairClimb stairClimb;

    public Vector3 moveInput;
    public Camera mainCamera;
    public bool isFreeze = false;
    public bool isInLadder = false;
    public bool isGrounded;
    //public bool isGroundedTopTower;
    public bool isInTopTower;

    private void Start()
    {
        playerController = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
        stairClimb = GetComponent<StairClimb>();
        mainCamera = Camera.main;
    }

    private void Awake()
    {
        GameEvent.OnFreezePlayer.AddListener(OnFreezePlayer);
        GameEvent.OnUnFreezePlayer.AddListener(OnUnFreezePlayer);
    }

    private void OnDestroy()
    {
        GameEvent.OnFreezePlayer.RemoveAllListeners();
        GameEvent.OnUnFreezePlayer.RemoveAllListeners();
    }

    private void OnFreezePlayer()
    {
        isFreeze = true;
    }

    private void OnUnFreezePlayer()
    {
        isFreeze = false;
    }

    private void Update()
    {
        GroundCheck();

        if (isFreeze)
        {
            Move(Vector3.zero);
            return;
        }

        //float inputMagnitude = moveInput.magnitude;

        //Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        //if (mainCamera != null)
        //{
        //    moveDirection = moveDirection.x * mainCamera.transform.right +
        //                  moveDirection.z * mainCamera.transform.forward;
        //    moveDirection.y = 0;

        //    moveDirection = moveDirection.normalized * inputMagnitude;
        //}

        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        if (mainCamera != null)
        {
            moveDirection = moveDirection.x * mainCamera.transform.right +
                          moveDirection.z * mainCamera.transform.forward;
            moveDirection.y = 0;
            moveDirection.Normalize();
        }

        if (!isInLadder)
        {
            Move(moveDirection);
            Rotate(moveDirection);
        }
    }

    protected void GroundCheck()
    {
        //var rayCheck = Physics.Raycast(groundCheckPoint.position, Vector3.up * -1, 1.0f, groundLayer);

        Collider[] colliders = Physics.OverlapBox(groundCheckPoint.position, groundCheckVector, Quaternion.identity, groundLayer);
        isGrounded = colliders.Length > 0;
 
    }


    public void Jump()
    {
        //if (playerController.isDead || isFreeze) return;

        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            //SFXManager.Instance.PlaySFX("jump", transform.position, 1.0f, 0);
        }
    }

    public void Move(Vector3 direction)
    {
        if (playerController.isDead) return;
        //Vector3 targetVelocity = direction * speed;
        //currentSpeed = targetVelocity.magnitude;

        Vector3 targetVelocity = direction * speed;

        rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);

        if (direction.magnitude >= 0.1f)
        {
            stairClimb.stepClimb();
        }
    }

    public virtual void Rotate(Vector3 direction)
    {
        if (playerController.isDead)
            return;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void ApplyForceDetection()
    {
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public void ResetDetection()
    {
        rb.interpolation = RigidbodyInterpolation.None;
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
    }


    protected void OnDrawGizmos()
    {
        if (groundCheckPoint == null) return;

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckVector * 2);
    }

    public void SetMoveInputVector(Vector2 moveInput)
    {
        this.moveInput = moveInput;
    }
}
