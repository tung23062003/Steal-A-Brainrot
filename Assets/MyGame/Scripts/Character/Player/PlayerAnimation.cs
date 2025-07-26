using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Animator animator;

    //private void Awake()
    //{
    //    GameEvent.OnChangeSkin.AddListener(() => animator = GetComponentInChildren<Animator>());
    //}

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }


    private void Update()
    {
        SetPlayerAnimation();
    }

    private void SetPlayerAnimation()
    {
        animator.SetBool(Animator.StringToHash("isGrounded"), playerMovement.isGrounded);

        animator.SetFloat("VelocityY", playerMovement.rb.velocity.y);

        animator.SetBool(Animator.StringToHash("isMoving"), new Vector3(playerMovement.rb.velocity.x, 0, playerMovement.rb.velocity.z).magnitude >= 0.1f && playerMovement.isGrounded);

        float clampedSpeed = Mathf.Clamp(playerMovement.currentSpeed, 0f, 3f);
        animator.SetFloat("MoveSpeed", clampedSpeed);

        
    }
}
