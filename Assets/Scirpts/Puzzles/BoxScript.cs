using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum states
{
    stationary,
    moving,
    falling
}
public class BoxScript : MonoBehaviour, IInteractable
{
    [SerializeField] private float speed;
    private Vector3 startPos;

    private states state;

    private void Awake()
    {
        state = states.stationary;
    }
    public void Interact(GameObject other)
    {
        Vector3 launchDir;
        float force;

        Rigidbody otherRb = other.GetComponent<Rigidbody>();

        if (Mathf.Abs(other.transform.forward.z) > Mathf.Abs(other.transform.forward.x))
        {
            launchDir = new Vector3(0, 0, other.transform.forward.normalized.z);
            force = otherRb.velocity.z/2;
        }
        else
        {
            launchDir = new Vector3(other.transform.forward.normalized.x, 0, 0);
            force = otherRb.velocity.x/2;
        }

        StartCoroutine(MovePosition(transform.position + (launchDir * Mathf.Abs(force))));
    }

    private IEnumerator MovePosition(Vector3 target)
    {
        state = states.moving;

        startPos = transform.position;
        float time = (Vector3.Distance(startPos, target) / speed)*Time.deltaTime;

        while ((transform.position.x != target.x || transform.position.z != target.z) && state == states.moving)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            transform.position = Vector3.Lerp(startPos,target,time);
            time += Time.deltaTime;
        }
        state = states.stationary;
    }
}
