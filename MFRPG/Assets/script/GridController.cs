using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    public enum CatWalkingState
    {
        Wait, Moving, MoveEnd
    }
    public static GridController instance;
    private Tilemap tilemap;

    public CatWalkingState catWalkingState = CatWalkingState.Wait;

    public List<GameObject> currentMapItems = new List<GameObject>();

    void Awake()
    {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        instance = this;
    }

    void Start()
    {
    }

    void Update()
    {

    }
    public void SetNowRound(){
        catWalkingState = CatWalkingState.Wait;
    }

    bool IsOutOfBound(Vector3Int currentGridPosition){
        TileBase selectedTile = tilemap.GetTile(currentGridPosition);
        if (selectedTile != null)
        {
            // Debug.Log("Selected tile: " + selectedTile.name + " at position: " + currentGridPosition);
            return false;
        }
        else
        {
            // Debug.Log("No tile at current position.");
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

    public void MoveAlso(GameObject objectToMove, Vector3Int targetGridPosition)
    {
        if (IsOutOfBound(targetGridPosition)) {
            return;
        }

        Vector3 targetWorldPosition = tilemap.CellToWorld(targetGridPosition);
        objectToMove.transform.position = targetWorldPosition;
    }

    public List<Vector3Int> FindPathBFS(Vector3Int start, Vector3Int target)
    {
        var path = new List<Vector3Int>();
        var queue = new Queue<Vector3Int>();
        var visited = new HashSet<Vector3Int>();
        var predecessors = new Dictionary<Vector3Int, Vector3Int>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            Vector3Int cur = queue.Dequeue();

            if (cur == target)
            {
                while (cur != start)
                {
                    path.Add(cur);
                    cur = predecessors[cur];
                }
                path.Add(start);
                path.Reverse();
                return path;
            }

            var neighbors = GetNeighbors(cur);
            foreach (var neighbor in neighbors)
            {
                bool isCantMove = false;
                Vector3 targetPosition = tilemap.CellToWorld(neighbor);
                float checkRadius = 0.1f;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(targetPosition, checkRadius);
                foreach (var collider in colliders)
                {
                    if ((collider.name == "Thief" || collider.name == "MapItem(Clone)") && neighbor != target)
                    {
                        isCantMove = true;
                    }
                }
                if (isCantMove)
                {
                    continue;
                }

                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                    predecessors[neighbor] = cur;
                }
            }
        }
        return null;
    }
    public List<Vector3Int> GetNeighbors(Vector3Int position)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        Vector3Int[] directions = {
            Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
        };

        foreach (var direction in directions)
        {
            Vector3Int neighborPos = position + direction;
            if (IsOutOfBound(neighborPos))
            {
                continue;
            }
            neighbors.Add(neighborPos);
        }

        return neighbors;
    }
    public Vector3Int?[] GetNeighborsArray(Vector3Int position)
    {
        Vector3Int?[] neighbors = new Vector3Int?[4] { null, null, null, null };

        Vector3Int[] directions = {
            Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
        };

        for (int i = 0; i < 4; i++)
        {
            Vector3Int neighborPos = position + directions[i];
            if (IsOutOfBound(neighborPos))
            {
                continue;
            }
            neighbors[i] = neighborPos;
        }

        return neighbors;
    }

    public IEnumerator MoveCorutine(GameObject objectToMove, List<Vector3Int> path) {
        catWalkingState = CatWalkingState.Moving;
        foreach (Vector3Int pos in path)
        {
            MoveAlso(objectToMove, pos);
            yield return new WaitForSeconds(0.3f);
        }
        catWalkingState = CatWalkingState.MoveEnd;
    }

    public Vector3Int GetGridPosition(GameObject objectToMove)
    {
        return tilemap.WorldToCell(objectToMove.transform.position);
    }

    public Vector3Int GetRandomGridPosition()
    {
        Vector3Int randomPosition = new Vector3Int(Random.Range(-5, 5), Random.Range(-10, 10), 0);
        while (IsOutOfBound(randomPosition))
        {
            randomPosition = new Vector3Int(Random.Range(-5, 5), Random.Range(-10, 10), 0);
        }
        return randomPosition;
    }

}
