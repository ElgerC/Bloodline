using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingGuard : GuardScript
{
    #region Patrolling Guard
    [Header ("Patrolling Guard")]
    [SerializeField] List<Vector3> basePatrolPoints = new List<Vector3>();
    [SerializeField] int basePatrolIndex;
    #endregion
    protected override void Start()
    {
        base.Start();
        EndPatrol();
    }

    protected override void BaseBehaviour()
    {
        Patrol();
    }

    protected override void EndPatrol()
    {
        patrolIndex = basePatrolIndex;
        RoamPoints = basePatrolPoints;
    }
}
