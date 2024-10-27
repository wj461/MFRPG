using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    string _name;
    int _cost;
    float _probability;
    int _catLike;
    public Sprite _sprite;

    public ItemDTO _itemDTO;
    // Start is called before the first frame update
    public void setItem(string name, int cost, float probability, int catLike, string imageName, ItemDTO itemDTO, bool isMapItem = false)
    {
        _name = name;
        _cost = cost;
        _probability = probability;
        _catLike = catLike;
        _sprite = Resources.Load<Sprite>("image/" + imageName.Trim());
        // Debug.Log("image/" + imageName);
        // _sprite = Resources.Load<Sprite>("image/youkai_jinmenken");
        // _sprite = Resources.Load<Sprite>("image/youkai_jinmenken");
        _itemDTO = itemDTO;
        if (isMapItem)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = _sprite;
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = _sprite;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PutItemInMap(Vector3 targetWorldPosition)
    {
        this.transform.position = targetWorldPosition;
    }

}
