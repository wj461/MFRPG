using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemDTO _itemDTO;
    // Start is called before the first frame update
    public void setItem(ItemDTO itemDTO, bool isMapItem = false)
    {
        Sprite sprite = Resources.Load<Sprite>("image/" + itemDTO.imagePath.Trim());
        _itemDTO = itemDTO;
        if (isMapItem)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = sprite;
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
        GameManager.instance.currentPlayer._cost -= _itemDTO.cost;
        // ThiefController.instance._cost -= _itemDTO.cost;
        this.transform.position = targetWorldPosition;
    }

    public void ThiefTouchItemAction()
    {
        ThiefController.instance._hp -= _itemDTO.ATK;
        Debug.Log("Touch item: " + _itemDTO.name);
    }

    //the event when this game object overlapping another game object
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D: " + other.name);
        if (other.name == "Thief")
        {
            Debug.Log("Thief touch item: " + _itemDTO.name);
            ThiefTouchItemAction();
            GridController.instance.currentMapItems.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
        else if (other.name == "Cat")
        {
            Debug.Log("Cat touch item: " + _itemDTO.name);
            GridController.instance.currentMapItems.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public float GetDistanceTo(GameObject other)
    {
        return Vector3.Distance(this.transform.position, other.transform.position);
    }

}
