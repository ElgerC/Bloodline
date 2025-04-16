using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardingEnemy : GuardScript
{
    [Header("Guarding")]
    [SerializeField] private Vector3 guardingPosition;
    [SerializeField] private Vector3 guardingDirection;

    [SerializeField] private AnimationCurve guardingCurve;
    [SerializeField] private float guardingAngle;
    [SerializeField] private float guardingTime;
    [SerializeField] private int guardDir = 1;

    protected override void Awake()
    {
        base.Awake();

        float guardAngleLeft = transform.rotation.eulerAngles.y - (guardingAngle / 2);
        float guardAngleRight = transform.rotation.eulerAngles.y + (guardingAngle / 2);

        Keyframe[] keyframes = guardingCurve.keys;

        keyframes[0].value = guardAngleLeft;
        keyframes[1].value = guardAngleRight;

        guardingCurve.keys = keyframes;

        guardingTime = guardingCurve.keys[1].time / 2;
    }

    protected override void Start()
    {
        base.Start();

        guardingDirection = transform.localRotation.eulerAngles;
        guardingPosition = transform.localPosition;
    }

    protected override void BaseBehaviour()
    {
        agent.SetDestination(guardingPosition);
        if (Vector3.Distance(transform.localPosition, guardingPosition) < 0.5)
        {
            animator.SetBool("Walking",false);

            agent.ResetPath();

            transform.rotation = Quaternion.Euler(0,guardingCurve.Evaluate(guardingTime),0);

            if (guardingTime + Time.deltaTime > guardingCurve.keys[1].time || guardingTime - Time.deltaTime < guardingCurve.keys[0].time)
            {
                guardDir -= guardDir * 2;
            }

            guardingTime += Time.deltaTime * guardDir;


        }
    }

    protected override void EndPatrol()
    {
        base.EndPatrol();
        agent.SetDestination(guardingPosition);
        guardingTime = guardingCurve.keys[1].time / 2;
    }
}
