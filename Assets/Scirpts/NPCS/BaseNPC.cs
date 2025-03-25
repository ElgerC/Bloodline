using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseNPC : MonoBehaviour, IInteractable
{
    #region general
    protected enum EnemyStates
    {
        baseBehaviour,
        allerted,
        LOSBroken,
        conectionPlayer,
        dead
    }

    [SerializeField] protected EnemyStates state;

    [SerializeField] private float moveSpeed;

    [SerializeField] private Collider col;

    #endregion
    #region Sight
    [SerializeField] private float viewAngle;

    [SerializeField] private float viewDistance;
    private GameObject player;
    [SerializeField] private LayerMask sightLayerMask;

    #endregion


    public void Interact(GameObject other)
    {
        state = EnemyStates.dead;
        col.enabled = false;
    }

    private void Awake()
    {
        state = EnemyStates.baseBehaviour;
        player = GameObject.FindGameObjectWithTag("Player");

        col = GetComponent<Collider>();
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (state != EnemyStates.dead && collision.transform.tag == "Player")
    //        state = EnemyStates.conectionPlayer;
    //}
    private void Update()
    {
        switch (state)
        {
            case EnemyStates.baseBehaviour:
                BaseBehaviour();

                if (VisionCheck())
                {
                    state = EnemyStates.allerted;
                }
                break;
            case EnemyStates.allerted:
                if (!VisionCheck())
                {
                    state = EnemyStates.baseBehaviour;
                }

                Allerted();
                break;
            case EnemyStates.LOSBroken:
                if (VisionCheck())
                {
                    state = EnemyStates.allerted;
                }

                LOSBroken();
                break;
            case EnemyStates.conectionPlayer:
                ConectionPlayer();
                break;
            default:
                break;
        }
    }

    protected virtual void BaseBehaviour()
    {

    }

    protected virtual void Allerted()
    {

    }

    protected virtual void LOSBroken()
    {

    }

    protected virtual void ConectionPlayer()
    {

    }

    private bool VisionCheck()
    {
        Vector3 dir = player.transform.position - transform.position;
        if (Vector3.Angle(transform.forward, dir) < viewAngle)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dir, out hit, viewDistance, sightLayerMask))
            {
                if (hit.transform.tag == "Player")
                {
                    return true;
                }
            }
            else if (state == EnemyStates.allerted)
            {
                state = EnemyStates.LOSBroken;
                return true;
            }
        }
        return false;
    }
}
