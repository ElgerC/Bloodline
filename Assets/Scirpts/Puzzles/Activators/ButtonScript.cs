using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : EnviromentLever
{
    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;

    [SerializeField] private bool pressed;

    [SerializeField] private AnimationCurve pressCurve;
    private float curveTime;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Keyframe[] keyframes = pressCurve.keys;

        keyframes[0].value = minHeight; keyframes[1].value = maxHeight;

        pressCurve.keys = keyframes;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Activate();

        pressed = true;
        transform.localPosition = new Vector3(0, minHeight, 0);
    }

    private void OnCollisionExit(Collision collision)
    {
        Deactivate();

        curveTime = 0;
        pressed = false;
    }

    protected override void Update()
    {
        base.Update();

        if (!permanent)
        {
            if (pressed)
            {
                if (transform.localPosition.y <= minHeight)
                {
                    rb.isKinematic = true;
                }
            }
            else if (transform.localPosition.y <= maxHeight)
            {
                rb.isKinematic = false;
                curveTime += Time.deltaTime;

                transform.localPosition = new Vector3(0, pressCurve.Evaluate(curveTime));
            }
        }
    }
}
