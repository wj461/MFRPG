using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("n") && !BannerController.instance.isShow()){
            this.gameObject.SetActive(false);
        }
    }
}
