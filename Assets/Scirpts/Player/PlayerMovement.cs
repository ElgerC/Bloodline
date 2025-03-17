using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region movement
    [Header("WASD Movement")]
    public bool canMove;
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

    [SerializeField] private float gravity;
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
    private Collider[] coliders;

    #region
    [Header("Dash")]
    public bool dashing = false;

    [SerializeField] private float dashDist;
    [SerializeField] private float performedDist;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashSlowdownDist;
    [SerializeField] private float dashCD;
    [SerializeField] private bool canDash;

    [SerializeField] private LayerMask normalLayerMask;
    [SerializeField] private LayerMask dashLayerMask;

    [SerializeField] private AnimationCurve dashCurve;


    //Dash check
    [SerializeField] private Vector3 checkBoxSizeHalf;
    [SerializeField] Transform checkBoxPos;

    private Vector3 lastPos;
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

        Keyframe[] keyframes = dashCurve.keys;

        keyframes[1].value = dashSpeed;
        keyframes[1].time = dashSlowdownDist;

        dashCurve.keys = keyframes;
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
        if (ctx.performed && grounded && !dashing)
        {
            //giving the player a boost of upwards momentum
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        }
    }
    public void Dash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !dashing && canDash)
        {
            performedDist = 0;
            lastPos = transform.position;
            dashing = true;

            for (int i = 0; coliders.Length > i; i++)
            {
                coliders[i].excludeLayers = dashLayerMask;
            }

            StartCoroutine(DashCD());
        }
    }

    private IEnumerator DashCD()
    {
        canDash = false;
        yield return new WaitForSeconds(5);
        canDash = true;
    }

    private void EndDash()
    {
        for (int i = 0; coliders.Length > i; i++)
        {
            coliders[i].excludeLayers = normalLayerMask;
        }

        dashing = false;
        rb.useGravity = true;
    }

    //Tutorial https://unity.com/blog/games/animation-curves-the-ultimate-design-lever 
    private float EvaluateDashCurve(float time)
    {
        return dashCurve.Evaluate(time);
    }

    public void Crouch(InputAction.CallbackContext ctx)
    {
        if (!dashing)
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
    }
    private void Update()
    {

        MoveCam();
    }

    private void FixedUpdate()
    {
        if (!dashing)
        {
            //Vector3 dir = new Vector3(Camera.main.transform.forward.x,0,Camera.main.transform.forward.z);

            //Calculating the players input to movement directions
            Vector3 curMoveZ = transform.forward.normalized * move.y * moveSpeed;
            Vector3 curMoveX = transform.right.normalized * move.x * moveSpeed;

            //Grabing the current upwards mom   entum to avoid freezing mid air
            Vector3 curVelo = new Vector3(0, rb.velocity.y, 0);

            //Setting the momentum to the above vectors

            if (Mathf.Abs(rb.velocity.magnitude) < Mathf.Abs((curMoveX + curMoveZ + curVelo).magnitude))
                rb.velocity = curMoveX + curMoveZ + curVelo;

            GroundCheck();
        }
        else
        {
            performedDist += Vector3.Distance(lastPos, transform.position);
            lastPos = transform.position;

            if (performedDist >= dashDist)
            {
                dashing = false;
            }
            else
            {
                if (performedDist < (dashDist - dashSlowdownDist))
                    rb.velocity = Camera.main.transform.forward * dashSpeed;
                else
                {
                    
                    float velo = EvaluateDashCurve(performedDist - (dashDist - dashSlowdownDist));
                    rb.velocity = Camera.main.transform.forward * velo;
                }
                    
            }
        }

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

            rb.velocity = Vector3.zero;
            EndDash();
        }
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
        yRotation += mouseX * Time.fixedDeltaTime;
        xRotation -= mouseY * Time.fixedDeltaTime;

        //Keeping the rotation between 2 values
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(0, yRotation, 0);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //setting the player and camera rotation to match with the saved ones
    }
}
