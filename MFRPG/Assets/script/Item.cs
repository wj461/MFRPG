using TMPro;
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
        TMP_Text text = transform.Find("cost")?.gameObject.GetComponent<TMP_Text>();
        if (text != null){
            text.text = _itemDTO.cost.ToString();
        }
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
        ThiefController.instance._hp -= _itemDTO.ATK * DefenderController.instance._atk;
    }

    //the event when this game object overlapping another game object
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Thief")
        {
            ThiefTouchItemAction();
            GridController.instance.currentMapItems.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
        else if (other.name == "Cat")
        {
            GridController.instance.currentMapItems.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public float GetDistanceTo(GameObject other)
    {
        return Vector3.Distance(this.transform.position, other.transform.position);
    }

}
