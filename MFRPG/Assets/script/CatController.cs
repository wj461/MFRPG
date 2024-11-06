using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatController : CanMove
{
    public enum CatState
    {
        CanMove,
        CanNotMove
    }
    public GameObject chess;
    public GameObject eventCG;
    public static CatController instance;
    public Vector3Int start = new Vector3Int(-5, -10, 0);

    public GameObject RTAGameObject;

    public GameObject movePreviewMid;

    public CatState catState = CatState.CanMove;
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        start = new Vector3Int(-5, -10, 0);
        GridController.instance.MoveAlso(this.gameObject, start);
        movePreviewMid = GameObject.Find("MovePreviewMidCat");
    }

    public void SetNowRound(){
        catState = CatState.CanMove;
        chess.SetActive(false);
        movePreviewMid.transform.position = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RandomCatSkipEvent(float probability = 30)
    {
        float random = UnityEngine.Random.Range(0, 100);
        if (random < probability)
        {
            catState = CatState.CanNotMove;
            chess.SetActive(true);
            eventCG.SetActive(true);
        }
    }

    public void CatMoveMotion(){
        if (catState == CatState.CanNotMove)
        {
            return;
        }

        GameObject mostLikeItem = FindMostLikeItem();
        Vector3Int moveDirection = new Vector3Int(0, 0, 0);
        if (mostLikeItem != null)
        {
            moveDirection = GridController.instance.GetGridPosition(mostLikeItem);
        }
        else
        {
            moveDirection = GetRandomGridPosition();
        }
        List<Vector3Int> path = GridController.instance.FindPathBFS(GridController.instance.GetGridPosition(this.gameObject), moveDirection);
        if (path != null){
            StartCoroutine(GridController.instance.MoveCorutine(this.gameObject, path));
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Thief")
        {
            RTAGameObject.SetActive(true);
        }
    }
}
