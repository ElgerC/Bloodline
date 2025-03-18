using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform exitPoint;
    public void Interact(GameObject other)
    {
        other.transform.position = exitPoint.position + exitPoint.transform.forward;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
