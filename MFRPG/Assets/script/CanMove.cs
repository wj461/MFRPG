using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GridController.instance.Move(this.gameObject, new Vector2Int(0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w")){
            GridController.instance.Move(this.gameObject, new Vector2Int(0, 1));
        }
        else if (Input.GetKeyDown("s")){
            GridController.instance.Move(this.gameObject, new Vector2Int(0, -1));
        }
        else if (Input.GetKeyDown("a")){
            GridController.instance.Move(this.gameObject, new Vector2Int(-1, 0));
        }
        else if (Input.GetKeyDown("d")){
            GridController.instance.Move(this.gameObject, new Vector2Int(1, 0));
        }
    }

}
