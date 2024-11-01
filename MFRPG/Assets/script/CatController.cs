using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : CanMove
{
    public Vector3Int start = new Vector3Int(0, 0, 0);
    public Vector3Int target = new Vector3Int(-5, -10, 0);


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            List<Vector3Int> path = GridController.instance.FindPathBFS(start, target);
            if (path == null)
            {
                Debug.Log("No path found.");
                return;
            }

            StartCoroutine(GridController.instance.MoveCorutine(this.gameObject, path));
        }
    }
}
