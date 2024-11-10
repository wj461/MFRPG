using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameController : MonoBehaviour
{
    public static NewGameController instance = null;
    public GameObject rule;
    public GameObject page1;
    public GameObject page2;
    public GameObject page3;
    public GameObject page4;
    public GameObject page5;

    public bool isGuideOver = false;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        isGuideOver = false;
    }
    void Start()
    {
        rule.SetActive(true);
        page1.SetActive(true);
        page2.SetActive(false);
        page3.SetActive(false);
        page4.SetActive(false);
        page5.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("n")){
            if (page1.activeInHierarchy){
                page1.SetActive(false);
                page2.SetActive(true);
            }
            else if (page2.activeInHierarchy){
                page2.SetActive(false);
                page3.SetActive(true);
            }
            else if (page3.activeInHierarchy){
                page3.SetActive(false);
                page4.SetActive(true);
            }
            else if (page4.activeInHierarchy){
                page4.SetActive(false);
                page5.SetActive(true);
            }
            else if (page5.activeInHierarchy){
                page5.SetActive(false);
                rule.SetActive(false);
                isGuideOver = true;
            }
        }
    }
}
