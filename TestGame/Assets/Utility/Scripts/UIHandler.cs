using InventorySystem;
using QuestSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

    public static bool isPaused = false;
    public Text qId;
    
    public void Cancel()
    {
        QuestManager.instance.questPanel.SetActive(false);
        Time.timeScale = 1f;

    }
    public void Accept(int id)
    {
        id = int.Parse(qId.GetComponent<Text>().text);
        QuestManager.instance.AcceptQuest(id);
        QuestManager.instance.questPanel.SetActive(false);
        Time.timeScale = 1f;

    }
    public void RemoveItem(string itemName)
    {
        itemName = transform.parent.name;
        if (int.Parse(transform.parent.GetChild(1).GetComponent<Text>().text) > 1)
        {
            transform.parent.GetChild(1).GetComponent<Text>().text = (int.Parse(transform.parent.GetChild(1).GetComponent<Text>().text) - 1).ToString();
            Inventory.instance.itemSpawner.SpawnItem(itemName);
        }
        else
        {            
            Inventory.instance.DropItem(itemName);
        }
    }
    public void UseItem(string itemName)
    {
        itemName = transform.name;

        ItemModel item = Inventory.instance.items.Find(p => p.itemName == itemName);
        
        
        int health = GameObject.Find("Player").GetComponent<Player>().health;
        int maxHealth = (int)GameObject.Find("Player").GetComponent<Player>().healthFill.maxValue;
        int healthPoints = item.itemPoints;


        if (item.itemType == ItemModel.ItemType.weapon_collectable)
            Inventory.instance.UseItem(itemName);


        if (int.Parse(transform.GetChild(1).GetComponent<Text>().text) > 1)
        {            
            if (item.itemType == ItemModel.ItemType.consumption_collectable)
            {
                if (health == maxHealth)
                    return;

                else if (health + healthPoints >= maxHealth)
                    GameObject.Find("Player").GetComponent<Player>().health = maxHealth;

                else
                    GameObject.Find("Player").GetComponent<Player>().health += healthPoints;

                transform.GetChild(1).GetComponent<Text>().text = (int.Parse(transform.GetChild(1).GetComponent<Text>().text) - 1).ToString();
            }
        }
        else
        {            
            if (item.itemType == ItemModel.ItemType.consumption_collectable)
            {
                if (health == maxHealth)
                    return;

                else if (health + healthPoints >= maxHealth)
                    GameObject.Find("Player").GetComponent<Player>().health = maxHealth;

                else
                    GameObject.Find("Player").GetComponent<Player>().health += healthPoints;

                Inventory.instance.UseItem(itemName);
            }
        }

    }
}
