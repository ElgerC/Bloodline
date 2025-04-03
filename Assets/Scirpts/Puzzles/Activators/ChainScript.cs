using System.Collections;
using UnityEngine;

public class ChainScript : EnviromentLever
{
    [SerializeField] private bool breakable;
    [SerializeField] private float holdDur;

    protected override void Start()
    {
        base.Start();

        if ( breakable)
        {
            permanent = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Active && other.gameObject.GetComponent<PlayerMovement>().dashing)
        {
            Activate();

            if (breakable)
            {
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(HoldTimer());
            }
        }
    }

    private IEnumerator HoldTimer()
    {
        yield return new WaitForSeconds(holdDur);

        Deactivate();
    } 
}
