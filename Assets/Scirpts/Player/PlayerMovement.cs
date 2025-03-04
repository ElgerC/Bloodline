using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpHeigt;
    private Vector2 move;

    [SerializeField] private float groundCheckRange;
    [SerializeField] private LayerMask groundCheckLayermask;
    [SerializeField] private bool grounded = false;
    

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MovePlayer(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            move = ctx.ReadValue<Vector2>().normalized;
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && grounded)
        {
            rb.AddForce(transform.up*jumpHeigt,ForceMode.Impulse);
        }
    }

    private void Update()
    {
        Vector3 curMove = new Vector3(move.x * moveSpeed,rb.velocity.y,move.y*moveSpeed);

        if(curMove.x != 0 || curMove.z != 0)
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
            } else
            {
                grounded = false;
            }
    }
}
