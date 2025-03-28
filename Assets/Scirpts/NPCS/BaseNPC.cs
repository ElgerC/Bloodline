using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class BaseNPC : MonoBehaviour, IInteractable
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

    [SerializeField] protected UnityEvent SightBroken = new UnityEvent();

    [SerializeField] protected EnemyStates state;

    [SerializeField] private float moveSpeed;

    [SerializeField] private Collider col;

    protected NavMeshAgent agent;

    #endregion
    #region Sight
    [SerializeField] private float viewAngle;

    [SerializeField] private float viewDistance;
    protected GameObject player;
    protected Vector3 lastPlayerPos;

    [SerializeField] protected LayerMask sightLayerMask;

    #endregion

    private void Awake()
    {
        state = EnemyStates.baseBehaviour;
        player = GameObject.FindGameObjectWithTag("Player");

        col = GetComponent<Collider>();

        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        agent.speed = moveSpeed;
    }

    public void Interact(GameObject other)
    {
        state = EnemyStates.dead;
        col.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != EnemyStates.dead && collision.transform.tag == "Player")
            state = EnemyStates.conectionPlayer;
    }
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
                lastPlayerPos = player.transform.position;

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
                else if (state == EnemyStates.allerted)
                {
                    SightBroken.Invoke();
                    state = EnemyStates.LOSBroken;
                    return true;
                }
            }

        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (player != null)
            Gizmos.DrawRay(transform.position, player.transform.position - transform.position);
    }
}
