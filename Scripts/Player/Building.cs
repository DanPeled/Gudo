using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Building : MonoBehaviour
{
    public GameObject[] placeable;
    public List<GameObject> placed = new List<GameObject>();
    public GameObject[] amountObjects;
    public GameObject rocks;
    public GameObject[] tools;
    public static Building instance;
    public int blockIndex = 0;
    public Slot[] inventory = new Slot[10];
    RaycastHit2D hit;
    public GameObject[] inventoryUI;
    public bool creative;
    public GameObject currentBlock;
    Movement player;
    public Item[] items;
    public GameObject none_;
    public static GameObject none;
    public Slot empty;
    public Camera cam;
    public Dictionary<float, GameObject> blocks = new Dictionary<float, GameObject>();
    public void PlaceByID(Vector2 pos){
        GameObject placed = new GameObject();
                    if (inventory[blockIndex].ID != 7)
                    {
                        placed = Instantiate(blocks[inventory[blockIndex].ID - 1]);
                        this.placed.Add(placed);
                    }
                    else
                    {
                        placed = Instantiate(rocks);
                        this.placed.Add(placed);
                    }
                    placed.transform.position = pos;

                    placed.name = string.Format("tile_x{0}_y{1}", placed.transform.position.x, placed.transform.position.y);
                    if(placed.GetComponent<Animator>() == null) hit.collider.gameObject.transform.position = new Vector3(1000,1000,1000);
                    Debug.Log(string.Format("Placed {0} at {1}", placed.name, pos));
                    if (!creative)
                        inventory[blockIndex].amount--;
    }
    private void Start()
    {
        none = none_;
        empty = new Slot("", "", Block.Rarity.None, 1, none, false, 0);
        player = GetComponent<Movement>();
        for (int i = 4; i < inventory.Length; i++)
        {
            inventory[i] = empty;
        }
        for (int i = 0; i < 4; i++)
        {
            inventory[i] = AddBlock(tools[i],
                tools[i].GetComponent<Item>().ItemName, 1,
                tools[i].GetComponent<Item>().description,
                tools[i].GetComponent<Item>().rarity, false, tools[i].GetComponent<Item>().ID);
        }
        for (int i = 1; i < placeable.Length; i++)
        {
            blocks.Add(i, placeable[i]);
        }
        blocks.Add(11, items[11].gameObject);
        inventory[0] = empty;
    }
    public Slot AddBlock(GameObject block, string blockName,
        int amount, string desc, Block.Rarity rarity, bool isBlock, float ID)
    {
        Slot target = new Slot(blockName, desc, rarity, amount, block, true, ID);
        return target;

    }
    void Update()
    {
        instance = this;
        if (Input.GetKeyDown(KeyCode.Q) && player.playerActive){
            GameObject drop = Instantiate(inventory[blockIndex].item);
            drop.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            drop.transform.position = new Vector3(drop.transform.position.x, drop.transform.position.y, 1f);
            inventory[blockIndex].amount--;}
        int inde = 0;
        foreach (Slot slot in inventory)
        {
            inde++;
            if (slot.amount <= 0)
            {
                inventory[inde - 1] = empty;
            }
        }
        #region Keys
        if(player.playerActive){
            if (Input.GetKeyDown(KeyCode.Alpha0)) blockIndex = 0; // Checks which number you click and sets the blockIndex var matching.
            if (Input.GetKeyDown(KeyCode.Alpha1)) blockIndex = 1;
            if (Input.GetKeyDown(KeyCode.Alpha2)) blockIndex = 2;
            if (Input.GetKeyDown(KeyCode.Alpha3)) blockIndex = 3;
            if (Input.GetKeyDown(KeyCode.Alpha4)) blockIndex = 4;
            if (Input.GetKeyDown(KeyCode.Alpha5)) blockIndex = 5;
            if (Input.GetKeyDown(KeyCode.Alpha6)) blockIndex = 6;
            if (Input.GetKeyDown(KeyCode.Alpha7)) blockIndex = 7;
            if (Input.GetKeyDown(KeyCode.Alpha8)) blockIndex = 8;
            if (Input.GetKeyDown(KeyCode.Alpha9)) blockIndex = 9;
            blockIndex += (int)Input.mouseScrollDelta.y;
            if (blockIndex < 0)
            {
                blockIndex = 9;
            }
            if (blockIndex > 9)
            {
                blockIndex = 0;
            }
        }
        #endregion
        #region Placing Blocks
        if (player.playerActive && player.playerState != Movement.PlayerState.Digging)
        {
            if (Input.GetMouseButtonDown(1) && inventory[blockIndex].amount >= 0)
            {
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                Vector2 pos = hit.collider.gameObject.transform.position;
                if (hit.collider.gameObject != gameObject &&
                 !hit.collider.gameObject.tag.Equals("Plant") &&
                  hit.collider.gameObject.layer != 5 && (
                   inventory[blockIndex].item.GetComponent<Block>() != null || inventory[blockIndex].ID == 11))
                {
                        PlaceByID(pos);
                }
            }


        }
        #endregion
        #region Image Updating
        for (int i = 0; i < inventory.Length; i++) // loops through the the inventory and setting the matcing images.
        {
            inventoryUI[i].GetComponent<Image>().sprite = inventory[i].item.GetComponent<SpriteRenderer>().sprite;
            inventoryUI[i].GetComponent<Image>().material = null;
            if(inventory[i].ItemName != ""){
                amountObjects[i].GetComponent<TMPro.TextMeshProUGUI>().text = inventory[i].amount.ToString();
            }
            else{
                amountObjects[i].GetComponent<TMPro.TextMeshProUGUI>().text = "";
            }
        }
        currentBlock.transform.position = inventoryUI[blockIndex].transform.position;
        #endregion
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
    #region Item Collection
        Item item = collision.gameObject.GetComponent<Item>();
        if(item != null){
            for (int i = 1; i < inventory.Length; i++)
            {
                if (inventory[i].ItemName == "")
                {
                    inventory[i] = AddBlock(
                        item.gameObject,
                        item.ItemName, 1,
                        item.description,
                        item.rarity,
                        item.gameObject.GetComponent<Block>() != null,
                        item.ID);
                    item.gameObject.transform.position = new Vector3(1000, 1000, 0);
                    break;
                }
                if (inventory[i].ItemName == item.ItemName)
                {
                    inventory[i].amount++;
                    item.gameObject.transform.position = new Vector3(1000,1000,1000);
                    Debug.Log(inventory[i].amount + " " + inventory[i].ItemName);
                    break;
                }
            }
        }
    #endregion
    }
}
[System.Serializable]
public class Slot
{
    public bool isBlock;
    public float ID = 0;
    public string ItemName;
    public string description;
    public Block.Rarity rarity = Block.Rarity.Common;
    public int amount;
    public GameObject item;

    public Slot(string ItemName, string description, Block.Rarity rarity, int amount, GameObject item, bool isBlock, float ID)
    {
        this.description = description;
        this.ItemName = ItemName;
        this.rarity = rarity;
        this.amount = amount;
        this.item = item;
        this.isBlock = isBlock;
        this.ID = ID;
    }
}
