using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatController : CanMove
{
    public Vector3Int start = new Vector3Int(-5, -10, 0);

    // Start is called before the first frame update
    void Start()
    {
        start = new Vector3Int(-5, -10, 0);
        Debug.Log("CatController start: " + start);
        GridController.instance.MoveAlso(this.gameObject, start);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("c")){
            GameObject mostLikeItem = FindMostLikeItem();
            if (mostLikeItem != null)
            {
                GridController.instance.MoveAlso(this.gameObject, GridController.instance.GetGridPosition(mostLikeItem));
            }
            else
            {
                GridController.instance.MoveAlso(this.gameObject, GetRandomGridPosition());
            }
        }
    }

    public Vector3Int GetRandomGridPosition()
    {
        Vector3Int randomGridPosition = GridController.instance.GetRandomGridPosition();
        Vector3Int thiefGridPosition = GridController.instance.GetGridPosition(ThiefController.instance.gameObject);
        Vector3Int currentGridPosition = GridController.instance.GetGridPosition(this.gameObject);
        float currentDistanceWithThief = Vector3Int.Distance(currentGridPosition, thiefGridPosition);
        float distance = Vector3Int.Distance(randomGridPosition, thiefGridPosition);
        while (distance < currentDistanceWithThief)
        {
            if (distance > 5)
            {
                return randomGridPosition;
            }
            randomGridPosition = GridController.instance.GetRandomGridPosition();
            distance = Vector3Int.Distance(randomGridPosition, thiefGridPosition);
        }
        return randomGridPosition;
    }


    public GameObject FindMostLikeItem()
    {
        GameObject mostLikeItem = null;
        int maxCatLike = -1;
        foreach (GameObject item in GridController.instance.currentMapItems)
        {
            Item itemScript = item.GetComponent<Item>();
            float distance = itemScript.GetDistanceTo(this.gameObject);
            int currentCatLike = (itemScript._itemDTO.catLike * 5) - ((int)distance * 2);
            Debug.Log("Item: " + itemScript._itemDTO.name + " CatLike: " + currentCatLike);
            if (currentCatLike > maxCatLike)
            {
                maxCatLike = currentCatLike;
                mostLikeItem = item;
            }
        }

        return mostLikeItem;
    }
}
