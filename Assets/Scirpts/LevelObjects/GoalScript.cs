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
            NextScene();
    }

    public void Trigger()
    {
        if (goalType == GoalType.target) NextScene();
    }

    private void NextScene()
    {
        GameManager.instance.LoadNextScene();
    }
}
