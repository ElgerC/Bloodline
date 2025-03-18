using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform exitPoint;
    BlackScreenScript BlackScreenScript;

    private GameObject target;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        BlackScreenScript = FindObjectOfType<BlackScreenScript>();
    }

    public void Interact(GameObject other)
    {
        animator.SetTrigger("Open");
        BlackScreenScript.Fade(1);

        other.GetComponent<PlayerMovement>().canMove = false;
        target = other;
    }

    public void Close()
    {
        target.GetComponent<PlayerMovement>().canMove = true;
        target.transform.position = exitPoint.position + (exitPoint.transform.forward*3);
        target.transform.forward = exitPoint.transform.forward;

        BlackScreenScript?.Fade(-1);
    }
}
