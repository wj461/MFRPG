using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMove : MonoBehaviour
{
    public GridController gridController;

    public Vector3Int currentGridPosition = new Vector3Int(0, 0, 0);
    public Vector3Int previousGridPosition = new Vector3Int(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        (currentGridPosition, previousGridPosition) = gridController.Move(this.gameObject, currentGridPosition, previousGridPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w")){
            currentGridPosition.y += 1; 
            _Move();
        }
        else if (Input.GetKeyDown("s")){
            currentGridPosition.y -= 1; 
            _Move();
        }
        else if (Input.GetKeyDown("a")){
            currentGridPosition.x -= 1; 
            _Move();
        }
        else if (Input.GetKeyDown("d")){
            currentGridPosition.x += 1; 
            _Move();
        }
    }

    private void _Move(){
        (currentGridPosition, previousGridPosition) = gridController.Move(this.gameObject, currentGridPosition, previousGridPosition);
    }
}
