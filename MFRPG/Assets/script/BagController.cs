using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
public class ItemDTO
{
    public string name { get; set; }
    public int cost { get; set; }
    public float probability { get; set; }
    public int catLike { get; set; }
    public string imagePath { get; set; }
    public int ATK { get; set; }
}

public enum BagState
{
    Close,
    Open
}

public class BagController : MonoBehaviour
{
    public static BagController instance;
    public Animator animator;
    public GameObject BagPanel;
    public TMP_Text BagText;

    public GameObject cursorGameObject;
    public Tilemap tilemap;
    public GameObject mapItemPrefab;
    public GameObject bagItemPrefab;
    public TextAsset itemDataTextAsset;
    public List<ItemDTO> itemBaseData = new List<ItemDTO>();
    public List<GameObject> currentBagItems = new List<GameObject>();
    private Color notSelectedColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);

    public BagState bagState = BagState.Close;

    private int currentSelectedIndex = -1;
    // Start is called before the first frame update

    void Awake()
    {
        instance = this;
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
            item.ATK = int.Parse(row[5]);

            itemBaseData.Add(item);
        }
        // this.gameObject.SetActive(false);
    }

    void Start()
    {
    }

    GameObject CreateMapItem(ItemDTO item)
    {
        GameObject itemObject = Instantiate(mapItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Item itemComponent = itemObject.GetComponent<Item>();
        itemComponent.setItem(item, true);
        return itemObject;
    }

    GameObject CreateBagItem(ItemDTO item)
    {
        GameObject itemObject = Instantiate(bagItemPrefab, transform);
        Item itemComponent = itemObject.GetComponent<Item>();
        itemComponent.setItem(item);
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
            if (IsCurrentSelectedIndexValid()){
                currentBagItems[currentSelectedIndex].GetComponent<Image>().color = Color.white;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    BagPutItemInMap();
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    currentBagItems[currentSelectedIndex].GetComponent<Image>().color = notSelectedColor;
                    currentSelectedIndex = (currentSelectedIndex - 1 + currentBagItems.Count) % currentBagItems.Count;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    currentBagItems[currentSelectedIndex].GetComponent<Image>().color = notSelectedColor;
                    currentSelectedIndex = (currentSelectedIndex + 1) % currentBagItems.Count;
                }
            }
            else{
                Close();
            }

        }
        
    }

    public void ShowText(){
        BagPanel.SetActive(true);
    }
    public void CloseText(){
        BagPanel.SetActive(false);
    }

    public void Close(){
        if (IsCurrentSelectedIndexValid()) currentBagItems[currentSelectedIndex].GetComponent<Image>().color = notSelectedColor;
        bagState = BagState.Close;
        currentSelectedIndex = -1;
        // gameObject.SetActive(false);
        cursorGameObject.SetActive(false);
        animator.SetBool("IsShow", false);
        BagText.text = "Open Bag";
    }

    public void Open(){
        bagState = BagState.Open;
        currentSelectedIndex = 0;
        if (IsCurrentSelectedIndexValid()) {
            // gameObject.SetActive(true);
            cursorGameObject.SetActive(true);
            animator.SetBool("IsShow", true);
            BagText.text = "Close Bag";
        }
        else {
            Close();
            animator.SetBool("IsShow", false);
            BagText.text = "Open Bag";
        }
    }

    public void BagPutItemInMap()
    {
        GameObject currentMapItem = CreateMapItem(GetItemComponent(currentBagItems[currentSelectedIndex])._itemDTO);
        Item currentMapItemScrip = GetItemComponent(currentMapItem);
        if (!IsCursorCanPutItem() || GameManager.instance.currentPlayer._cost < currentMapItemScrip._itemDTO.cost){
            Destroy(currentMapItem);
            return;
        }

        var targetWorldPosition = cursorGameObject.transform.position;

        GridController.instance.currentMapItems.Add(currentMapItem);
        currentMapItemScrip.PutItemInMap(targetWorldPosition);

        // After put then del
        Destroy(currentBagItems[currentSelectedIndex]);
        currentBagItems.RemoveAt(currentSelectedIndex);
        currentSelectedIndex = 0;
    }

    public void AddItem(string itemName)
    {
        foreach (ItemDTO item in itemBaseData)
        {
            if (item.name == itemName)
            {
                currentBagItems.Add(CreateBagItem(item));
                break;
            }
        }
    }
    public ItemDTO CreateItemDTO(string itemName)
    {
        foreach (ItemDTO item in itemBaseData)
        {
            if (item.name == itemName)
            {
                return item;
            }
        }
        return null;
    }

    public ItemDTO CreateRandomItemDTO()
    {
        float random = UnityEngine.Random.Range(0, 100);
        float sum = 0.0f;
        while (true)
        {
            foreach (ItemDTO item in itemBaseData)
            {
                sum += item.probability;
                if (sum >= random)
                {
                    return item;
                }
            }
        }
    }
    
    public bool IsCursorCanPutItem()
    {
        BoxCollider2D boxCollider = cursorGameObject.GetComponent<BoxCollider2D>();
        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCollider.bounds.center, boxCollider.bounds.size, 0);
        foreach (var hit in hits)
        {
            if (hit.gameObject.name != "Cursor")
            {
                return false;
            }
        }
        return true;
    }

    public void SaveCurrentPlayerBagItems(){
        GameManager.instance.currentPlayer._items = new List<ItemDTO>();
        for (int i = 0; i < currentBagItems.Count; i++)
        {
            GameManager.instance.currentPlayer._items.Add(currentBagItems[i].GetComponent<Item>()._itemDTO);
            Destroy(currentBagItems[i]);
        }
    }
    public void SetCurrentBagItems(){
        var items = GameManager.instance.currentPlayer._items;

        currentBagItems = new List<GameObject>();
        foreach (ItemDTO item in items)
        {
            currentBagItems.Add(CreateBagItem(item));
        }
        currentSelectedIndex = -1;
        bagState = BagState.Close;
    }

}
