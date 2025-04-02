using UnityEngine;

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
    [SerializeField] protected bool permanent;
    [SerializeField] private GameObject controllingObject;
    [SerializeField] protected bool Active;

    [SerializeField] private bool trigger;

    [Header("Moving")]
    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private float elapsedTime;

    [SerializeField] private Vector3 goalPosition;
    [SerializeField] private Vector3 startPosition;

    [Header("Unlocking")]
    [SerializeField] Unlockable unlockable;

    private Rigidbody controllingRB;

    protected virtual void Start()
    {
        startPosition = controllingObject.transform.localPosition;

        Unlockable temp = controllingObject.GetComponent<Unlockable>();
        if (temp != null)
        {
            unlockable = temp;
        }

        Rigidbody tempRB = controllingObject.GetComponent<Rigidbody>();
        if (tempRB != null)
        {
            controllingRB = tempRB;
        }
        
    }
    protected virtual void Activate()
    {
        if(!Active) 
        {
            switch (consequence)
            {
                case Consequences.Moving:
                    break;
                case Consequences.Unlocking:
                    unlockable.SetLock(true);
                    break;
                case Consequences.Dropping:
                    controllingRB.isKinematic = false;
                    break;
            }

            Active = true;
        }
    }

    protected virtual void Deactivate()
    {
        if(Active)
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
    }

    protected virtual void Update()
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

                    controllingObject.transform.localPosition = pos;
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

                    controllingObject.transform.localPosition = pos;
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
