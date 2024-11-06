using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEditor.PackageManager;
using UnityEditor.Rendering;
using UnityEngine;

[SerializeField]
public interface Player
{
    // Start is called before the first frame update
    string _playerName { get; set; }
    int _hp { get; set; }
    int _cost { get; set; }
    int _atk { get; set; }

    List<ItemDTO> _items { get; set; }
    
    public void Player(string name, int hp, int atk, int cost,  List<ItemDTO> items){
        _playerName = name;
        _hp = hp;
        _atk = atk;
        _cost = cost;
        _items = items;
    }
}
