using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimController : MonoBehaviour
{
    public static Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void AnimControl()
    {
        animator.SetBool("isWalking", true);
    }
}
