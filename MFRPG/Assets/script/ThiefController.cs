using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public enum ThiefState
{
    CanMove,
    CanNotMove
}

public class ThiefController : CanMove, Player
{
    public static ThiefController instance;
    public string _playerName { get; set; }
    public int _hp { get; set; }
    public int _cost { get; set; }
    public Item[] _items { get; set; }
    public PlayerBuff[] _buffs { get; set; }
    public Vector3Int start = new Vector3Int(0, 0, 0);

    public ThiefState thiefState = ThiefState.CanMove;


    public GameObject[] movePreview = new GameObject[4];
    public GameObject movePreviewMid;

    public Sprite heartSprite;
    public Sprite heartSpriteEmpty;
    public Sprite costSprite;
    public Sprite costSpriteEmpty;
    
    public Image[] hearts = new Image[10];

    public Image[] costs = new Image[6];

    public void Player(string name, int hp, int cost, Item[] items, PlayerBuff[] buffs)
    {
        _playerName = name;
        _hp = hp;
        _cost = cost;
        _items = items;
        _buffs = buffs;
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        movePreview[0] = GameObject.Find("MovePreviewUp");
        movePreview[1] = GameObject.Find("MovePreviewDown");
        movePreview[2] = GameObject.Find("MovePreviewLeft");
        movePreview[3] = GameObject.Find("MovePreviewRight");
        movePreviewMid = GameObject.Find("MovePreviewMid");

        GridController.instance.MoveAlso(this.gameObject, start);
        GridController.instance.MoveAlso(movePreviewMid, start);
        ReSetMovePreview();
        _hp = 10;
        _cost = 6;
        for (int i = 1; i < 11; i++)
        {
            hearts[i-1] = GameObject.Find("Heart" + i).GetComponent<Image>();
            hearts[i-1].sprite = heartSprite;
        }
        for (int i = 1; i < 7; i++)
        {
            costs[i-1] = GameObject.Find("Cost" + i).GetComponent<Image>();
            costs[i-1].sprite = heartSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHeart();
        UpdateCost();
        if (thiefState == ThiefState.CanMove && _cost > 0)
        {
            if (Input.GetKeyDown("w")){
                GridController.instance.Move(this.gameObject, new Vector2Int(0, 1));
                ReSetMovePreview();
                _cost -= 1;
            }
            else if (Input.GetKeyDown("s")){
                GridController.instance.Move(this.gameObject, new Vector2Int(0, -1));
                ReSetMovePreview();
                _cost -= 1;
            }
            else if (Input.GetKeyDown("a")){
                GridController.instance.Move(this.gameObject, new Vector2Int(-1, 0));
                ReSetMovePreview();
                _cost -= 1;
            }
            else if (Input.GetKeyDown("d")){
                GridController.instance.Move(this.gameObject, new Vector2Int(1, 0));
                ReSetMovePreview();
                _cost -= 1;
            }
        }
        
    }

    public void CloseMove(){
        thiefState = ThiefState.CanNotMove;
        CloseAllMovePreview();
    }

    public void OpenMove(){
        thiefState = ThiefState.CanMove;
        ReSetMovePreview();
    }

    void ReSetMovePreview()
    {
        var currentPosition = GridController.instance.GetPosition(this.gameObject);
        var neighborPath = GridController.instance.GetNeighborsArray(currentPosition);
        for (int i = 0; i < 4; i++)
        {
            if (neighborPath[i] == null)
            {
                movePreview[i].SetActive(false);
            }
            else
            {
                movePreview[i].SetActive(true);
                GridController.instance.MoveAlso(movePreview[i], neighborPath[i].Value);
            }
        }
        movePreviewMid.SetActive(true);
        GridController.instance.MoveAlso(movePreviewMid, currentPosition);
    }

    void CloseAllMovePreview()
    {
        for (int i = 0; i < 4; i++)
        {
            movePreview[i].SetActive(false);
        }
        movePreviewMid.SetActive(false);
    }

    void UpdateHeart()
    {
        for (int i = 0; i < 10; i++)
        {
            if (i < _hp)
            {
                hearts[i].sprite = heartSprite;
            }
            else
            {
                hearts[i].sprite = heartSpriteEmpty;
            }
        }
    }
    void UpdateCost()
    {
        for (int i = 0; i < 6; i++)
        {
            if (i < _cost)
            {
                costs[i].sprite = costSprite;
            }
            else
            {
                costs[i].sprite = costSpriteEmpty;
            }
        }
    }

}
