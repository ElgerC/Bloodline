using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
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

    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    private float xRotation;
    private float yRotation;

    [SerializeField] Vector3 test;


    private Animator animator;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        test = transform.forward;

        Vector3 curMoveZ = transform.forward.normalized * move.y * moveSpeed;
        Vector3 curMoveX = transform.right.normalized * move.x * moveSpeed;

        Vector3 curVelo = new Vector3(0, rb.velocity.y, 0);

        if (curMoveX.x != 0 || curMoveZ.z != 0)
        {
            rb.velocity = curMoveX + curMoveZ + curVelo;
        }

        GroundCheck();

        MoveCam();
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

    private void MoveCam()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        Debug.Log(xRotation);
        xRotation = Mathf.Clamp(xRotation, -90, 90);


        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation,0, 0);
    }
}
