using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardingEnemy : GuardScript
{
    [Header("Guarding")]
    [SerializeField] private Vector3 guardingPosition;
    [SerializeField] private Vector3 guardingDirection;
    
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
            agent.ResetPath();
            transform.rotation = Quaternion.Euler(guardingDirection);
        }
    }

    protected override void EndPatrol()
    {
        base.EndPatrol();
        agent.SetDestination(guardingPosition);
    }
}
