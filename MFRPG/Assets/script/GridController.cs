using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    public Tilemap tilemap;

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

    public (Vector3Int, Vector3Int) Move(GameObject objectToMove, Vector3Int targetGridPosition, Vector3Int previousGridPosition)
    {
        Debug.Log("Selected tile: " + targetGridPosition + previousGridPosition);
        Vector3Int currentGridPosition = targetGridPosition;
        if (IsOutOfBound(currentGridPosition)) currentGridPosition = previousGridPosition;
        Debug.Log("current tile: " + currentGridPosition);

        Vector3 targetWorldPosition = tilemap.CellToWorld(currentGridPosition);
        objectToMove.transform.position = targetWorldPosition;
        previousGridPosition = currentGridPosition;
        return (currentGridPosition, previousGridPosition);
    }
}
