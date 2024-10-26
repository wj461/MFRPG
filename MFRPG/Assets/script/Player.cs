using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEditor.PackageManager;
using UnityEditor.Rendering;
using UnityEngine;

public class Player
{
    // Start is called before the first frame update
    private string _playerName {set; get;}
    private int _hp {set; get;}
    private int _atk {set; get;}

    Item[] itemList;

    PlayerBuff[] buffList;
    
    public Player(string name, int hp, int atk){
        _playerName = name;
        _hp = hp;
        _atk = atk;
    }
}
