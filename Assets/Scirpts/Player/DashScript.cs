using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

public class DashScript : MonoBehaviour
{
    #region Dash 
    [Header("Dash")]
    //dash base
    [SerializeField] private float dashDist;
    [SerializeField] private float dashDur;
    [SerializeField] private float remainingDist;
    [SerializeField] private float endingDur;
    private float frameDist;

    [SerializeField] private LayerMask normalLayerMask;
    [SerializeField] private LayerMask dashLayerMask;

    private bool dashing = false;

    //Dash check
    [SerializeField] private Vector3 checkBoxSizeHalf;
    [SerializeField] Transform checkBoxPos;
    #endregion

    private Collider[] coliders;
    private Rigidbody rb;

    private void Awake()
    {
        //asignment
        coliders = GetComponents<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public void StartDash(InputAction.CallbackContext ctx)
    {
        if(ctx.performed && !dashing)
        {
            frameDist = (dashDist / dashDur) * Time.fixedDeltaTime;
        }
    }

    private void Update()
    {
        if (dashing)
        {
            
        }
    }

    //public void Dash(InputAction.CallbackContext ctx)
    //{
    //    if (ctx.performed && !dashing)
    //    {
    //        frameDist = (dashDist / dashDur) * Time.fixedDeltaTime;

    //        StartCoroutine(DashTimer());
    //    }
    //}

    //IEnumerator DashTimer()
    //{
    //    dashing = true;
    //    rb.useGravity = false;

    //    for (int i = 0; coliders.Length > i; i++)
    //    {
    //        coliders[i].excludeLayers = dashLayerMask;
    //    }
    //    yield return new WaitForSeconds(dashDur);
    //    dashing = false;
    //    rb.useGravity = true;
    //    for (int i = 0; coliders.Length > i; i++)
    //    {
    //        coliders[i].excludeLayers = normalLayerMask;
    //    }
    //}
    //private void FixedUpdate()
    //{
    //    Collider[] hits = Physics.OverlapBox(checkBoxPos.position, checkBoxSizeHalf, Camera.main.transform.rotation, groundCheckLayermask);

    //    if (hits.Length > 0 && dashing)
    //    {
    //        for (int i = 0; i < hits.Length; i++)
    //        {
    //            IInteractable interact = hits[i].GetComponent<IInteractable>();
    //            if (interact != null)
    //            {
    //                interact.Interact(gameObject);
    //            }
    //        }

    //        dashing = false;
    //        rb.useGravity = true;

    //        StopCoroutine(DashTimer());

    //    }
    //    if (dashing)
    //    {
    //        Vector3 dir = new Vector3(Camera.main.transform.forward.normalized.x * frameDist, 0, transform.forward.normalized.z * frameDist);
    //        rb.AddForce(Camera.main.transform.forward.normalized * frameDist, ForceMode.VelocityChange);
    //        //rb.MovePosition(transform.position + dir);
    //    }
    //}

    private void OnDrawGizmos()
    {
        if (dashing)
            Gizmos.DrawCube(checkBoxPos.position, checkBoxSizeHalf * 2);
    }
}
