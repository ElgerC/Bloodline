using UnityEngine;

public class Unlockable : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void SetLock(bool lockMode)
    {
        animator.SetBool("Unlocked", lockMode);
    }
}
