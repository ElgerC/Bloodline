using System.Collections;
using System.Linq;
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
    [SerializeField] private Collider[] coliders;

    #region Dash 
    [Header("Dash")]
    //dash base
    [SerializeField] private float dashDist;
    [SerializeField] private float dashDur;
    [SerializeField] private float endingDur;
    private float frameDist;

    [SerializeField] private LayerMask normalLayerMask;
    [SerializeField] private LayerMask dashLayerMask;

    private bool dashing = false;

    //Dash check
    [SerializeField] private Vector3 checkBoxSizeHalf;
    [SerializeField] Transform checkBoxPos;
    #endregion
    private void Awake()
    {
        //asignment
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        coliders = GetComponents<Collider>();

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
        if (ctx.performed && !dashing)
        {
            frameDist = (dashDist / dashDur) * Time.fixedDeltaTime;

            StartCoroutine(DashTimer());

            MoveCam();
        }
    }

    IEnumerator DashTimer()
    {
        dashing = true;
        rb.useGravity = false;

        for (int i = 0; coliders.Length > i; i++)
        {
            coliders[i].excludeLayers = dashLayerMask;
        }
        yield return new WaitForSeconds(dashDur);
        dashing = false;
        rb.useGravity = true;
        for (int i = 0; coliders.Length > i; i++)
        {
            coliders[i].excludeLayers = normalLayerMask;
        }
    }
    private void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapBox(checkBoxPos.position, checkBoxSizeHalf, Camera.main.transform.rotation, groundCheckLayermask);

        if (hits.Length > 0 && dashing)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                IInteractable interact = hits[i].GetComponent<IInteractable>();
                if (interact != null)
                {
                    interact.Interact(gameObject);
                }
            }

            dashing = false;
            rb.useGravity = true;

            StopCoroutine(DashTimer());

        }
        if (dashing)
        {
            Vector3 dir = new Vector3(Camera.main.transform.forward.normalized.x * frameDist, 0, transform.forward.normalized.z * frameDist);
            rb.AddForce(Camera.main.transform.forward.normalized * frameDist, ForceMode.VelocityChange);
            //rb.MovePosition(transform.position + dir);
        }
    }

    private void Update()
    {
        if (!dashing)
        {
            //Calculating the players input to movement directions
            Vector3 curMoveZ = transform.forward.normalized * move.y * moveSpeed;
            Vector3 curMoveX = transform.right.normalized * move.x * moveSpeed;

            //Grabing the current upwards momentum to avoid freezing mid air
            Vector3 curVelo = new Vector3(0, rb.velocity.y, 0);

            //Setting the momentum to the above vectors

            if (Mathf.Abs(rb.velocity.magnitude) < Mathf.Abs((curMoveX + curMoveZ + curVelo).magnitude))
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
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY;

        //removing the input of the saved rotation
        yRotation += mouseX * Time.deltaTime;
        xRotation -= mouseY * Time.deltaTime;

        //Keeping the rotation between 2 values
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(0, yRotation, 0);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //setting the player and camera rotation to match with the saved ones
    }

    private void OnDrawGizmos()
    {
        if (dashing)
            Gizmos.DrawCube(checkBoxPos.position, checkBoxSizeHalf * 2);
    }
}
