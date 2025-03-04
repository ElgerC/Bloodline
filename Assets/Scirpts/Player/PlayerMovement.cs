using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    private float jumpHeight;

    [SerializeField] private float normMoveSpeed;
    [SerializeField] private float normJumpHeight;

    [SerializeField] private float crouchMoveSpeed;
    [SerializeField] private float crouchJumpHeight;
    private Vector2 move;

    [SerializeField] private float groundCheckRange;
    [SerializeField] private LayerMask groundCheckLayermask;
    [SerializeField] private bool grounded = false;



    private Animator animator;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        moveSpeed = normMoveSpeed;
        jumpHeight = normJumpHeight;
    }
    public void MovePlayer(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            move = ctx.ReadValue<Vector2>().normalized;
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && grounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        }
    }

    public void Crouch(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            animator.SetBool("IsCrouching", true);
            moveSpeed = crouchMoveSpeed;
            jumpHeight = crouchJumpHeight;
        } else if (ctx.canceled)
        {
            animator.SetBool("IsCrouching", false);
            moveSpeed = normMoveSpeed;
            jumpHeight = normJumpHeight;
        }
    }

    private void Update()
    {
        Vector3 curMove = new Vector3(move.x * moveSpeed, rb.velocity.y, move.y * moveSpeed);

        if (curMove.x != 0 || curMove.z != 0)
        {
            rb.velocity = curMove;
        }

        GroundCheck();
    }

    private void GroundCheck()
    {
        RaycastHit[] hit = Physics.RaycastAll(transform.position, -transform.up, groundCheckRange, groundCheckLayermask);
        if (hit.Length > 0)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
}
