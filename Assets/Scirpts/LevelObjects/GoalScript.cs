using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GoalType
{
    pickUp,
    target
}

public class GoalScript : MonoBehaviour
{
    [SerializeField] private GoalType goalType;
    private void OnTriggerEnter(Collider other)
    {
        if (goalType == GoalType.pickUp)
            GameManager.instance.LoadNextScene();
    }

    private void OnDestroy()
    {
        if(goalType == GoalType.target) GameManager.instance.LoadNextScene();
    }
}
