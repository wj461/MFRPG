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

    List<ItemDTO> _items { get; set; }
    List<PlayerBuff> _buffs { get; set; }
    
    public void Player(string name, int hp, int cost,  List<ItemDTO> items, List<PlayerBuff> buffs){
        _playerName = name;
        _hp = hp;
        _cost = cost;
        _items = items;
        _buffs = buffs;
    }
}
