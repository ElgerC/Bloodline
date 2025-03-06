using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour, IInteractable
{
    [SerializeField] private float speed;
    private Vector3 startPos;


    public void Interact(GameObject other)
    {
         StartCoroutine(MovePosition(transform.position + other.transform.forward.normalized * 8));
    }

    private IEnumerator MovePosition(Vector3 target)
    {
        startPos = transform.position;
        float time = (Vector3.Distance(startPos, target) / speed)*Time.deltaTime;

        while (transform.position.x != target.x || transform.position.z != target.z)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            transform.position = Vector3.Lerp(startPos,target,time);
            time += Time.deltaTime;
        }
    }
}
