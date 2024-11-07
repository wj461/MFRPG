using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerController : MonoBehaviour
{
    public static BannerController instance;
    public Animator animator;

    public Vector3 startPosition;

    void Awake()
    {
        instance = this;
        startPosition = this.gameObject.transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsShow", false);
    }

    // Update is called once per frame
    void Update()
    {
        //當動畫撥放完畢時
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            HideBanner();
        }
    }

    public void HideBanner()
    {
        animator.SetBool("IsShow", false);
        this.gameObject.transform.position = startPosition;
    }

    public void ShowBanner()
    {
        animator.SetBool("IsShow", true);
    }

    public bool isShow()
    {
        return animator.GetBool("IsShow");
    }
}
