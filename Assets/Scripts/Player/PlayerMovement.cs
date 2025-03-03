using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody rb;
    private Vector2 move;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MovePlayer(InputAction.CallbackContext ctx)
    {
        Debug.Log("test");
        if (ctx.performed)
        {
            Debug.Log("grah");
            move = ctx.ReadValue<Vector2>();
        }
    }
    void Update()
    {
        rb.velocity = move*moveSpeed;
    }
}
