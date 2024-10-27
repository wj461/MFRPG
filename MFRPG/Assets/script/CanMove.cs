using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMove : MonoBehaviour
{
    public GridController gridController;

    // Start is called before the first frame update
    void Start()
    {
        gridController.Move(this.gameObject, new Vector2Int(0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w")){
            gridController.Move(this.gameObject, new Vector2Int(0, 1));
        }
        else if (Input.GetKeyDown("s")){
            gridController.Move(this.gameObject, new Vector2Int(0, -1));
        }
        else if (Input.GetKeyDown("a")){
            gridController.Move(this.gameObject, new Vector2Int(-1, 0));
        }
        else if (Input.GetKeyDown("d")){
            gridController.Move(this.gameObject, new Vector2Int(1, 0));
        }
    }

}
