using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    public Animator Animator => animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void UpdateSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }
}
