using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GuardScript : BaseNPC
{
    [Header("General")]
    [SerializeField] private float attackDist;

    #region Searching
    [Header ("Searching")]
    [SerializeField] int CheckAmount;

    [SerializeField] protected List<Vector3> RoamPoints;

    [SerializeField] protected int patrolIndex = 0;
    #endregion
    protected override void Start()
    {
        SightBroken.AddListener(GenerateCheckPoints);

        base.Start();
    }
    protected override void Allerted()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= attackDist && state != EnemyStates.dead)
        {
            state = EnemyStates.conectionPlayer;
            agent.ResetPath();
        }

        agent.SetDestination(player.transform.position);
    }

    protected override void LOSBroken()
    {
        Patrol();
    }

    public void GenerateCheckPoints()
    {
        List<Vector3> checkPositions = new List<Vector3>();

        for (int i = 0; i < CheckAmount; i++)
        {
            int rndX = Random.Range(-5, 5);
            int rndZ = Random.Range(-5, 5);

            Vector3 tstPos = new Vector3(lastPlayerPos.x + rndX, lastPlayerPos.y, lastPlayerPos.z + rndZ);

            NavMesh.SamplePosition(tstPos, out NavMeshHit hit, 15, sightLayerMask);

            checkPositions.Add(hit.position);
        }

        patrolIndex = 0;
        RoamPoints = checkPositions;
        StartCoroutine(PatrolTimer());
    }

    protected void Patrol()
    {
        if(patrolIndex > RoamPoints.Count) patrolIndex = 0;

        if (Vector3.Distance(transform.position, RoamPoints[patrolIndex]) < 2)
        {
            if (patrolIndex == RoamPoints.Count - 1) 
            {
                EndPatrol();
            }
            else
            {
                patrolIndex += 1;
            }
        }

        agent.SetDestination(RoamPoints[patrolIndex]);
    }

    protected virtual void EndPatrol()
    {
        patrolIndex = 0;
        state = EnemyStates.baseBehaviour;
    }

    private IEnumerator PatrolTimer()
    { 
        yield return new WaitForSeconds(10);
        EndPatrol();
    }
}
