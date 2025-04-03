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

    [SerializeField] protected float moveSpeed;

    [SerializeField] private Collider col;

    protected NavMeshAgent agent;

    #endregion
    #region Sight
    [SerializeField] private float viewAngle;

    [SerializeField] protected float viewDistance;
    protected GameObject player;
    protected GameObject POI;
    protected Vector3 lastPlayerPos;

    [SerializeField] protected LayerMask sightLayerMask;

    #endregion

    protected virtual void Awake()
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
        OnDeath();
        Destroy(gameObject);
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
                lastPlayerPos = POI.transform.position;

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

    protected virtual void OnDeath()
    {

    }

    protected virtual bool VisionCheck()
    {
        Vector3 dir = player.transform.position - transform.position;
        if (Vector3.Angle(transform.forward, dir) < viewAngle)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dir, out hit, viewDistance, sightLayerMask))
            {
                if (hit.transform.tag == "Player")
                {
                    POI = player;
                    return true;
                }
                else if (state == EnemyStates.allerted)
                {
                    POI = null;
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

    public virtual void Allert()
    {

    }
    public void Distract(GameObject obj)
    {
        Debug.Log("distracted");

        POI = obj;
        lastPlayerPos = POI.transform.position;
        SightBroken.Invoke();
        state = EnemyStates.LOSBroken;
    }

}
