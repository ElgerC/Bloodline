using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilianScript : BaseNPC
{
    #region runningAway
    [Header("Alerted")]
    [SerializeField] private GameObject[] exitPoints;

    #endregion

    #region baseBehaviour
    [Header("BaseBehaviour")]
    [SerializeField] private GameObject roamPoint;
    [SerializeField] private float roamDist;

    [SerializeField] private bool reasignPos;
    [SerializeField] private bool timerActive;

    #endregion

    protected override void BaseBehaviour()
    {
        if (reasignPos)
        {
            agent.SetDestination(FindPos());
            reasignPos = false;
        }
        else if (Vector3.Distance(transform.position, agent.destination)<1f && !timerActive) 
        {
            Debug.Log("Test");
            StartCoroutine(ReasignPosTimer(Random.Range(0, 8)));
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
}
