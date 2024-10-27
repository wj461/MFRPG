using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class ItemDTO
{
    public string name { get; set; }
    public int cost { get; set; }
    public float probability { get; set; }
    public int catLike { get; set; }
    public string imagePath { get; set; }
}

public enum BagState
{
    Close,
    Open
}

public class BagController : MonoBehaviour
{
    public GameObject cursorGameObject;
    public Tilemap tilemap;
    public GameObject mapItemPrefab;
    public GameObject bagItemPrefab;
    public TextAsset itemDataTextAsset;
    public List<ItemDTO> itemBaseData = new List<ItemDTO>();
    public List<GameObject> currentBagItems = new List<GameObject>();
    public List<GameObject> currentMapItemsInMap = new List<GameObject>();

    private Color selectedColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);

    public BagState bagState = BagState.Close;

    private int currentSelectedIndex = -1;
    // Start is called before the first frame update
    void Start()
    {
        // read item data
        string[] data = itemDataTextAsset.text.Split(new char[] { '\n' });
        for (int i = 1; i < data.Length-1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });
            ItemDTO item = new ItemDTO();
            item.name = row[0];
            item.cost = int.Parse(row[1]);
            item.probability = float.Parse(row[2]);
            item.catLike = int.Parse(row[3]);
            item.imagePath = row[4];

            itemBaseData.Add(item);
        }
        currentBagItems.Add(CreateBagItem(itemBaseData[1]));
        currentBagItems.Add(CreateBagItem(itemBaseData[0]));
        currentBagItems.Add(CreateBagItem(itemBaseData[1]));
        currentBagItems.Add(CreateBagItem(itemBaseData[0]));
    }

    GameObject CreateMapItem(ItemDTO item)
    {
        GameObject itemObject = Instantiate(mapItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Item itemComponent = itemObject.GetComponent<Item>();
        itemComponent.setItem(item.name, item.cost, item.probability, item.catLike, item.imagePath, item, true);
        return itemObject;
    }

    GameObject CreateBagItem(ItemDTO item)
    {
        GameObject itemObject = Instantiate(bagItemPrefab, transform);
        Item itemComponent = itemObject.GetComponent<Item>();
        itemComponent.setItem(item.name, item.cost, item.probability, item.catLike, item.imagePath, item);
        return itemObject;
    }

    Item GetItemComponent(GameObject itemObject)
    {
        return itemObject.GetComponent<Item>();
    }

    public bool IsCurrentSelectedIndexValid()
    {
        return currentSelectedIndex >= 0 && currentSelectedIndex < currentBagItems.Count && currentBagItems.Count > 0;
    }

    public bool CanPutItem(Vector3 targetWorldPosition)
    {
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        if (BagState.Open == bagState)
        {
            if (currentBagItems.Count == 0)
            {
                Close();
            }

            if (IsCurrentSelectedIndexValid()){
                currentBagItems[currentSelectedIndex].GetComponent<Image>().color = selectedColor;
            }
            if (Input.GetKeyDown(KeyCode.Space) && IsCurrentSelectedIndexValid())
            {
                BagPutItem();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if(IsCurrentSelectedIndexValid()) currentBagItems[currentSelectedIndex].GetComponent<Image>().color = Color.white;
                currentSelectedIndex = (currentSelectedIndex - 1 + currentBagItems.Count) % currentBagItems.Count;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (IsCurrentSelectedIndexValid()) currentBagItems[currentSelectedIndex].GetComponent<Image>().color = Color.white;
                currentSelectedIndex = (currentSelectedIndex + 1) % currentBagItems.Count;
            }
        }
        
    }

    public void Close(){
        if (IsCurrentSelectedIndexValid()) currentBagItems[currentSelectedIndex].GetComponent<Image>().color = Color.white;
        bagState = BagState.Close;
        currentSelectedIndex = -1;
        gameObject.SetActive(false);
    }

    public void Open(){
        bagState = BagState.Open;
        currentSelectedIndex = 0;
        if (IsCurrentSelectedIndexValid()) gameObject.SetActive(true);
        else Close();
    }

    public void BagPutItem()
    {
        GameObject currentMapItem = CreateMapItem(GetItemComponent(currentBagItems[currentSelectedIndex])._itemDTO);
        var targetWorldPosition = cursorGameObject.transform.position;
        if (!CanPutItem(targetWorldPosition)){
            return;
        }

        // currentMapItemsInMap.Add(currentMapItem);
        GetItemComponent(currentMapItem).PutItemInMap(targetWorldPosition);

        // After put then del
        Destroy(currentBagItems[currentSelectedIndex]);
        currentBagItems.RemoveAt(currentSelectedIndex);
        currentSelectedIndex = 0;
    }

    public void AddItem(Item item)
    {
        // for (int i = 0; i < currentItems.Length; i++)
        // {
        //     if (currentItems[i] == null)
        //     {
        //         currentItems[i] = item;
        //         break;
        //     }
        // }
    }

    public void RemoveItem(Item item)
    {
        // for (int i = 0; i < currentItems.Length; i++)
        // {
        //     if (currentItems[i] == item)
        //     {
        //         currentItems[i] = null;
        //         break;
        //     }
        // }
    }
}
