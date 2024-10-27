using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    private Tilemap tilemap;
    private Grid grid;

    void Awake()
    {
        grid = gameObject.GetComponent<Grid>();
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    }

    void Start()
    {
    }

    void Update()
    {

    }

    bool IsOutOfBound(Vector3Int currentGridPosition){
        TileBase selectedTile = tilemap.GetTile(currentGridPosition);
        if (selectedTile != null)
        {
            Debug.Log("Selected tile: " + selectedTile.name);
            return false;
        }
        else
        {
            Debug.Log("No tile at current position.");
            return true;
        }
    }

    public void Move(GameObject objectToMove, Vector2Int relationGridPosition)
    {
        Vector3Int currentGridPosition = tilemap.WorldToCell(objectToMove.transform.position);
        Vector3Int targetGridPosition = new Vector3Int(currentGridPosition.x + relationGridPosition.x, currentGridPosition.y + relationGridPosition.y, currentGridPosition.z);

        if (IsOutOfBound(targetGridPosition)) {
            return;
        }

        Vector3 targetWorldPosition = tilemap.CellToWorld(targetGridPosition);
        objectToMove.transform.position = targetWorldPosition;
    }

}
