using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum states
{
    stationary,
    moving
}
public class BoxScript : MonoBehaviour, IInteractable
{
    [SerializeField] private float speed;
    [SerializeField] private float minSpeed;

    private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    public void Interact(GameObject other)
    {
        Vector3 launchDir;
        float force;
        body.isKinematic = false;

        Rigidbody otherRb = other.GetComponent<Rigidbody>();

        if (Mathf.Abs(other.transform.forward.z) > Mathf.Abs(other.transform.forward.x))
        {
            launchDir = new Vector3(0, 0, other.transform.forward.normalized.z);
            force = otherRb.velocity.z;
        }
        else
        {
            launchDir = new Vector3(other.transform.forward.normalized.x, 0, 0);
            force = otherRb.velocity.x;
        }

        body.AddForce(launchDir * Mathf.Abs(force) * body.mass, ForceMode.Impulse);
    }
}
