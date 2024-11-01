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

    int _ATK;
    public Sprite _sprite;

    public ItemDTO _itemDTO;
    // Start is called before the first frame update
    public void setItem(string name, int cost, float probability, int catLike, string imageName, ItemDTO itemDTO, int ATK, bool isMapItem = false)
    {
        _name = name;
        _cost = cost;
        _probability = probability;
        _catLike = catLike;
        _sprite = Resources.Load<Sprite>("image/" + imageName.Trim());
        _itemDTO = itemDTO;
        if (isMapItem)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = _sprite;
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = _sprite;
        }
        _ATK = ATK;
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
        ThiefController.instance._cost -= _cost;
        this.transform.position = targetWorldPosition;
    }

    public void ThiefTouchItemAction()
    {
        ThiefController.instance._hp -= _ATK;
        Debug.Log("Touch item: " + _name);
    }

    //the event when this game object overlapping another game object
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Thief")
        {
            ThiefTouchItemAction();
            Destroy(this.gameObject);
        }
    }

}
