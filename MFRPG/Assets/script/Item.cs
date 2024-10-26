using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    string _name;
    int _cost;
    float _probability;
    int _catLike;
    // Start is called before the first frame update
    public void setItem(string name, int cost, float probability, int catLike)
    {
        _name = name;
        _cost = cost;
        _probability = probability;
        _catLike = catLike;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
