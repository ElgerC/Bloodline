using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerMovement : MonoBehaviour
{
    #region movement
    [Header("WASD Movement")]
    [SerializeField] private float normMoveSpeed;
    [SerializeField] private float crouchMoveSpeed;

    private float moveSpeed;

    private Vector2 move;
    [Header("Jumping")]
    [SerializeField] private float normJumpHeight;
    [SerializeField] private float crouchJumpHeight;

    private float jumpHeight;

    [SerializeField] private float groundCheckRange;
    [SerializeField] private LayerMask groundCheckLayermask;
    private bool grounded = false;
    #endregion

    #region camera
    [Header("Camera")]
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    private float xRotation;
    private float yRotation;
    #endregion

    //Components
    private Animator animator;
    private Rigidbody rb;

    [SerializeField] private bool canMove;

    [SerializeField] private float dashDist;
    [SerializeField] private float dashDur;

    private bool dashing = false;

    private void Awake()
    {
        //asignment
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        //setting the cursor and hiding it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Setting variables
        moveSpeed = normMoveSpeed;
        jumpHeight = normJumpHeight;
    }
    public void MovePlayer(InputAction.CallbackContext ctx)
    {
        //If button W,A,S or D is pressed set move
        if (ctx.performed)
        {
            move = ctx.ReadValue<Vector2>().normalized;
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        //Checking if the player is grounded
        if (ctx.performed && grounded)
        {
            //giving the player a boost of upwards momentum
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        }
    }

    public void Crouch(InputAction.CallbackContext ctx)
    {
        //when the button is held
        if (ctx.started)
        {
            //animating and slowing the players jump and movement
            animator.SetBool("IsCrouching", true);
            moveSpeed = crouchMoveSpeed;
            jumpHeight = crouchJumpHeight;
        }
        //When button is released
        else if (ctx.canceled)
        {
            //animating and reseting the players speed
            animator.SetBool("IsCrouching", false);
            moveSpeed = normMoveSpeed;
            jumpHeight = normJumpHeight;
        }
    }

    public void Dash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            StartCoroutine(DashTimer());
        }
    }

    IEnumerator DashTimer()
    {
        dashing = true;
        yield return new WaitForSeconds(dashDur);
        dashing = false;
    }

    private void Update()
    {
        float framerate = 1 / Time.smoothDeltaTime;

        if (dashing)
        {
            float frameDist
            //transform.position += transform.forward * dashSpeed * Time.deltaTime;
        } else
        {
            //Calculating the players input to movement directions
            Vector3 curMoveZ = transform.forward.normalized * move.y * moveSpeed;
            Vector3 curMoveX = transform.right.normalized * move.x * moveSpeed;

            //Grabing the current upwards momentum to avoid freezing mid air
            Vector3 curVelo = new Vector3(0, rb.velocity.y, 0);

            //Setting the momentum to the above vectors
            rb.velocity = curMoveX + curMoveZ + curVelo;
        }


        GroundCheck();

        MoveCam();
    }

    private void GroundCheck()
    {
        //Checking for objects below the player and setting a bool if yes
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

    /// <summary>
    /// Moving the camera and player with the mouse to simulate a 1st person experience
    /// </summary>
    private void MoveCam()
    {
        //Inspired by https://www.youtube.com/watch?v=f473C43s8nE

        //Getting the direction the mouse is moving in
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        //removing the input of the saved rotation
        yRotation += mouseX;
        xRotation -= mouseY;

        //Keeping the rotation between 2 values
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        //setting the player and camera rotation to match with the saved ones
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
