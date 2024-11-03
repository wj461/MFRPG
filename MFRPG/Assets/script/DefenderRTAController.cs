using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderRTAController : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("playOver"))
        {
            Destroy(this.gameObject);
        }
    }
}
