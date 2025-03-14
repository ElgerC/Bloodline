using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

enum Consequences
{
    Moving,
    Unlocking,
    Dropping
}
public class EnviromentLever : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Consequences consequence;
    [SerializeField] private bool permanent;
    [SerializeField] private GameObject controllingObject;
    [SerializeField] private bool Active;

    [SerializeField] private bool trigger;

    [Header("Moving")]
    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private float elapsedTime;

    [SerializeField] private Vector3 goalPosition;
    [SerializeField] private Vector3 startPosition;

    [Header("Unlocking")]
    [SerializeField] Unlockable unlockable;

    private void Start()
    {
        startPosition = controllingObject.transform.position;

        Unlockable temp = controllingObject.GetComponent<Unlockable>();
        if (temp != null)
        {
            unlockable = temp;
        }
        
    }
    protected virtual void Activate()
    {
        Active = true;

        switch (consequence)
        {
            case Consequences.Moving:
                break;
            case Consequences.Unlocking:
                unlockable.SetLock(true);
                break;
            case Consequences.Dropping:
                break;
        }
    }

    protected virtual void Deactivate()
    {
        Active = false;

        if (!permanent)
        {
            switch (consequence)
            {
                case Consequences.Moving:
                    break;
                case Consequences.Unlocking:
                    unlockable.SetLock(false);
                    break;
                case Consequences.Dropping:
                    break;
            }
        }
    }

    private void Update()
    {
        if (trigger)
        {
            if (Active)
            {
                Deactivate();
            }
            else
            {
                Activate();
            }
            trigger = false;
        }

        if (Active)
        {
            switch (consequence)
            {
                case Consequences.Moving:
                    if (elapsedTime < moveCurve.keys[moveCurve.length - 1].time)
                        elapsedTime += Time.deltaTime;

                    Vector3 pos = Vector3.Lerp(startPosition, goalPosition, EvaluateMoveCurve(elapsedTime));

                    controllingObject.transform.position = pos;
                    break;
                case Consequences.Unlocking:
                    break;
                case Consequences.Dropping:
                    break;
            }
        }
        else
        {
            switch (consequence)
            {
                case Consequences.Moving:
                    if (elapsedTime > 0)
                        elapsedTime -= Time.deltaTime;

                    Vector3 pos = Vector3.Lerp(startPosition, goalPosition, EvaluateMoveCurve(elapsedTime));

                    controllingObject.transform.position = pos;
                    break;
                case Consequences.Unlocking:
                    break;
                case Consequences.Dropping:
                    break;
            }
        }
    }

    private float EvaluateMoveCurve(float time)
    {
        return moveCurve.Evaluate(time);
    }
}
