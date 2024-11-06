using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class DefenderController : MonoBehaviour, Player
{
    public static DefenderController instance;
    public string _playerName { get; set; }
    public int _hp { get; set; }
    public int _cost { get; set; }
    public int _atk { get; set; }
    public List<ItemDTO> _items { get; set; }

    public Sprite costSprite;
    public Sprite costSpriteEmpty;
    

    public Image[] costs = new Image[6];

    public void SetPlayer(string name, int hp, int cost, int atk, List<ItemDTO> items)
    {
        _playerName = name;
        _hp = hp;
        _atk = atk;
        _cost = cost;
        _items = items;
    }

    public void SetNowRound(){
        _cost = 0;
        _items.Add(BagController.instance.CreateRandomItemDTO());
    }

    public void SetNewMatch(){
        _cost = 0;
        _items = new List<ItemDTO>();
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _hp = 10;
        _cost = 0;
        for (int i = 1; i < 7; i++)
        {
            costs[i-1] = GameObject.Find("CostD" + i).GetComponent<Image>();
            costs[i-1].sprite = costSpriteEmpty;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCost();
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
