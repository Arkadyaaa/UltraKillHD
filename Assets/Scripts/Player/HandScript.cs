using UnityEngine;

public class HandScript : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayShootAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Shoot");
            Debug.Log("Shoot Anim");
        }
    }

    public void PlayPunchAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Punch");
            Debug.Log("Punch Anim");
        }
    }
}
