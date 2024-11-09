using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q")){
            if (animator.GetBool("IsShow"))
            {
                animator.SetBool("IsShow", false);
            }
            else
            {
                animator.SetBool("IsShow", true);
            }
        }
    }
}
