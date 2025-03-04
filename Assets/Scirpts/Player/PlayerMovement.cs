using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpHeigt;
    private Vector2 move; 

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
        if (ctx.performed)
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
    }
}
