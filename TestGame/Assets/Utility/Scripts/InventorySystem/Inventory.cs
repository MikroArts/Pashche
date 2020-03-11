using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory instance;
        public InventoryDatabase database;
        public GameObject inventoryPanel;
        public GameObject slotPanel;
        public GameObject inventorySlot;
        public GameObject inventoryItem;
        public ItemSpawner itemSpawner;

        public Text itemAmountText;
        public Text itemIdTxt;
        int itemAmount = 1;

        public int slotAmount;
        public List<ItemModel> items = new List<ItemModel>();
        public List<GameObject> slots = new List<GameObject>();


        public GameObject infoPanel;
        public GameObject pickUpText;

        public bool equiped = false;

        void Awake()
        {
            instance = this;
        }
        void Start()
        {
            database = GetComponent<InventoryDatabase>();
            slotAmount = 20;

            FillSlots();
        }

        private void FillSlots()
        {
            for (int i = 0; i < slotAmount; i++)
            {
                items.Add(new ItemModel());
                slots.Add(Instantiate(inventorySlot));
                slots[i].transform.SetParent(slotPanel.transform);
                slots[i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }

        public void ShowInfoPanel()
        {
            infoPanel.SetActive(true);
            Invoke("CloseInfoPanel", 5f);
        }
        void CloseInfoPanel()
        {
            infoPanel.SetActive(false);
        }

        public void AddItem(int id)
        {
            ItemModel itemToAdd = database.ReturnItemById(id);

            for (int i = 0; i < slots.Count; i++)
            {
                if (CheckForItem(itemToAdd) && slots[i].name == itemToAdd.itemName)
                {
                    itemAmountText = slots[i].GetComponentInChildren<Text>();
                    itemAmountText.text = (int.Parse(itemAmountText.text) + 1).ToString();
                    return;
                }
            }

            for (int i = 0; i < items.Count; i++)
            {

                if (items[i].itemId < 0)
                {                    
                    items[i] = itemToAdd;

                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.transform.position = Vector2.zero;
                    itemIdTxt.text = id.ToString();
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.parent.name = itemToAdd.itemName;
                    itemObj.GetComponent<Image>().rectTransform.anchoredPosition = Vector2.zero;
                    itemObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + itemToAdd.spriteName);
                    itemObj.transform.localScale = new Vector3(.8f, .8f, .8f);
                    itemAmountText = slots[i].GetComponentInChildren<Text>();
                    itemAmountText.text = itemAmount.ToString();
                    itemObj.transform.name = itemToAdd.itemName;
                    return;
                }
            }

        }
        public void DropItem(string itemName)
        {
            items.Remove(database.ReturnItemByName(itemName));

            for (int i = 0; i < slotAmount; i++)
            {
                if (slotPanel.transform.GetChild(i).name == itemName)
                {
                    Destroy(slotPanel.transform.GetChild(i).gameObject);
                    slots.Remove(slots[i]);
                    items.Add(new ItemModel());
                    GameObject go = Instantiate(inventorySlot);
                    slots.Add(go);
                    go.transform.SetParent(slotPanel.transform);
                    go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                }
            }
            itemSpawner.SpawnItem(itemName);
        }
        public void UseItem(string itemName)
        {
            if (itemName == "Sword")
            {
                EquipWeapon(itemName);
                return;
            }
            items.Remove(database.ReturnItemByName(itemName));
            for (int i = 0; i < slotAmount; i++)
            {
                if (slotPanel.transform.GetChild(i).name == itemName)
                {
                    Destroy(slotPanel.transform.GetChild(i).gameObject);
                    slots.Remove(slots[i]);
                    items.Add(new ItemModel());
                    GameObject go = Instantiate(inventorySlot);
                    slots.Add(go);
                    go.transform.SetParent(slotPanel.transform);
                    go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                }

            }

        }

        public void EquipWeapon(string itemName)
        {
            if (!equiped)
            {
                foreach (var i in items.ToArray())
                {
                    if (i.itemName == itemName)
                    {
                        if (int.Parse(itemAmountText.text) >= 1)
                        {
                            GameObject item = itemSpawner.SpanGameObject(itemName);
                            item.GetComponent<MeshCollider>().enabled = false;
                            item.GetComponent<BoxCollider>().enabled = false;
                            item.GetComponent<CapsuleCollider>().enabled = false;
                            Destroy(item.GetComponent<Rigidbody>());
                            item.transform.SetParent(GameObject.Find("hand.R.001_end").transform);
                            item.transform.localPosition = Vector3.zero;
                            item.transform.localRotation = Quaternion.Euler(new Vector3(0,0,-45f));
                            equiped = true;
                            itemAmountText.text = (int.Parse(itemAmountText.text) - 1).ToString();
                        }
                        if (int.Parse(itemAmountText.text) < 1)
                        {
                            RemoveItem(itemName);
                        }
                    }
                }
            }

        }
        public void DequipWeapon(string weaponName)
        {
            if (equiped)
            {
                GameObject go = GameObject.Find("hand.R.001_end").GetComponentInChildren<PickUp>().gameObject;
                Destroy(go);
                AddItem(go.GetComponent<PickUp>().item.itemId);
                equiped = false;
            }
        }
        public void RemoveItem(string itemName)
        {
            items.Remove(database.ReturnItemByName(itemName));

            for (int i = 0; i < slotAmount; i++)
            {
                if (slotPanel.transform.GetChild(i).name == itemName)
                {
                    Destroy(slotPanel.transform.GetChild(i).gameObject);
                    slots.Remove(slots[i]);
                    items.Add(new ItemModel());
                    GameObject go = Instantiate(inventorySlot);
                    slots.Add(go);
                    go.transform.SetParent(slotPanel.transform);
                    go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                }
            }
            if (itemName != "Sword")
            {
                itemSpawner.SpawnItem(itemName);
            }
        }

        bool CheckForItem(ItemModel item)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (item.itemName == slots[i].name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}