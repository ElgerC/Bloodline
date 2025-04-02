using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilianScript : BaseNPC
{
    #region runningAway
    [Header("RunningAway")]
    [SerializeField] private LayerMask screamMask;
    [SerializeField] private float screamDist;

    [SerializeField] private bool running;
    [SerializeField] private GameObject[] exitPoints;
    [SerializeField] private float runTime;
    [SerializeField] private float runSpeed;
    #endregion

    #region backingOff
    [Header("Alerted")]
    [SerializeField] private float slowedSpeed;
    #endregion

    #region baseBehaviour
    [Header("BaseBehaviour")]
    [SerializeField] private GameObject roamPoint;
    [SerializeField] private float roamDist;

    [SerializeField] private bool reasignPos;
    [SerializeField] private bool timerActive;

    private Vector3 roamGoal;
    #endregion

    protected override void BaseBehaviour()
    {
        if (!running)
        {
            agent.SetDestination(roamGoal);
            agent.speed = moveSpeed;

            if (reasignPos)
            {
                roamGoal = FindPos();
                reasignPos = false;
            }
            else if (Vector3.Distance(transform.position, roamGoal) < 2 && !timerActive)
            {

                StartCoroutine(ReasignPosTimer(1));
            }
        }
        else if (Vector3.Distance(transform.position,agent.destination) < 1)
        {
            Destroy(gameObject);
        }
    }

    private Vector3 FindPos()
    {
        Vector3 pos = Vector3.zero;

        while (pos == Vector3.zero)
        {

            Vector3 tempPos = new Vector3(roamPoint.transform.position.x + Random.Range(-roamDist, roamDist), roamPoint.transform.position.y, roamPoint.transform.position.z + Random.Range(-roamDist, roamDist));

            NavMesh.SamplePosition(tempPos, out NavMeshHit hit, 5, sightLayerMask);

            if (Vector3.Distance(roamPoint.transform.position, hit.position) <= roamDist)
            {
                pos = hit.position;
            }
        }
        return pos;
    }

    private IEnumerator ReasignPosTimer(float time)
    {
        timerActive = true;
        yield return new WaitForSeconds(time);
        reasignPos = true;
        timerActive = false;
    }

    protected override void Allerted()
    {
        agent.speed = slowedSpeed;

        transform.LookAt(player.transform);
        if (Vector3.Distance(transform.position, player.transform.position) < viewDistance - 1)
        {
            Vector3 pos = transform.position + -transform.forward;
            agent.SetDestination(pos);
        }
    }

    protected override void LOSBroken()
    {
        agent.SetDestination(roamGoal);
        reasignPos = false;
        state = EnemyStates.baseBehaviour;
    }

    protected override void OnDeath()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, screamDist, screamMask);
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].GetComponent<BaseNPC>().Allert();
        }
    }

    public override void Allert()
    {
        GameObject exitPoint = exitPoints[0];
        for (int i = 0; i < exitPoints.Length; i++)
        {
            if (Vector3.Distance(transform.position, exitPoints[i].transform.position) < Vector3.Distance(transform.position, exitPoint.transform.position))
            {
                exitPoint = exitPoints[i];
            }
        }

        state = EnemyStates.baseBehaviour;
        StartCoroutine(RunTime());
        agent.SetDestination(exitPoint.transform.position);
    }

    private IEnumerator RunTime()
    {
        agent.speed = runSpeed;
        running = true;
        yield return new WaitForSeconds(runTime);
        running = false;
        agent.speed = moveSpeed;
    }

    protected override bool VisionCheck()
    {
        if (!running)
            return base.VisionCheck();
        else 
            return false;
    }
}
