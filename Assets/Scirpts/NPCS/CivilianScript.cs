using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilianScript : BaseNPC
{
    #region runningAway
    [Header("RunningAway")]
    [SerializeField] private GameObject[] exitPoints;
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
        agent.SetDestination(roamGoal);
        agent.speed = moveSpeed;

        if (reasignPos)
        {
            roamGoal = FindPos();
            reasignPos = false;
        }
        else if (Vector3.Distance(transform.position, roamGoal)<2 && !timerActive) 
        {
            
            StartCoroutine(ReasignPosTimer(1));
        }
    }

    private Vector3 FindPos()
    { 
        Vector3 pos = Vector3.zero;

        while (pos == Vector3.zero) 
        {
            
            Vector3 tempPos = new Vector3(roamPoint.transform.position.x + Random.Range(-roamDist, roamDist),roamPoint.transform.position.y, roamPoint.transform.position.z + Random.Range(-roamDist, roamDist));

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

        Vector3 pos = transform.position + -transform.forward;

        agent.SetDestination(pos);
    }
}
