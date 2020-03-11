using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class PickUp : MonoBehaviour
    {
        private bool isTriggering = false;
        public Player player;
        Inventory inventory;
        public Item item;
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            inventory = GameObject.Find("InventoryManager").GetComponent<Inventory>();
        }

        void Update()
        {
            if (isTriggering)
            {
                PickUpPrompt();
            }
        }

        public void PickUpPrompt()
        {
            if (gameObject.CompareTag("Collectible") && Input.GetButtonDown("Action") && item.itemName.ToUpper() == gameObject.name.ToUpper())
            {
                player.GetComponent<Animator>().SetBool("IsTake", true);
                print("PLAY PickUp Animation for " + gameObject.name);
                PickUpItem(item);
                inventory.pickUpText.SetActive(false);
            }
            else if (Input.GetButtonDown("Action"))
            {
                switch (gameObject.tag)
                {
                    case "Coin":
                        player.GetComponent<Animator>().SetBool("IsTake", true);
                        inventory.pickUpText.GetComponent<Text>().text = "Pick up\n" + name.ToUpper();
                        inventory.pickUpText.SetActive(true);
                        player.coin += 1;
                        Destroy(gameObject);
                        break;
                    case "CoinBag":
                        player.GetComponent<Animator>().SetBool("IsTake", true);
                        inventory.pickUpText.GetComponent<Text>().text = "Pick up\n" + name.ToUpper();
                        inventory.pickUpText.SetActive(true);
                        player.coin += 10;
                        Destroy(gameObject);
                        break;
                    case "CoinPile":
                        player.GetComponent<Animator>().SetBool("IsTake", true);
                        inventory.pickUpText.GetComponent<Text>().text = "Pick up\n" + name.ToUpper();
                        inventory.pickUpText.SetActive(true);
                        player.coin += 25;
                        Destroy(gameObject);
                        break;
                    case "CoinChest":
                        player.GetComponent<Animator>().SetBool("IsTake", true);
                        inventory.pickUpText.GetComponent<Text>().text = "Pick up\n" + name.ToUpper();
                        inventory.pickUpText.SetActive(true);
                        player.coin += 50;
                        Destroy(gameObject);
                        break;
                    default:
                        player.GetComponent<Animator>().SetBool("IsTake", false);
                        inventory.pickUpText.SetActive(false);
                        break;
                }

            }
        }

        public void PickUpItem(Item item)
        {
            
            int count = 0;
            for (int i = 0; i < inventory.items.Count; i++)
            {                
                if (inventory.items[i].itemId == -1)
                {                    
                    count++;
                }
            }
            if (count > 0)
            {                
                inventory.AddItem(item.itemId);
                Destroy(gameObject);
                return;
            }
            else
            {
                foreach (var slot in inventory.slots)
                {
                    if (slot.name == item.itemName)
                    {
                        inventory.AddItem(item.itemId);
                        Destroy(gameObject);
                        return;
                    }
                    else
                    {
                        inventory.infoPanel.GetComponentInChildren<Text>().text = "Inventory is full!";
                        inventory.infoPanel.GetComponent<Image>().color = Color.red;
                        inventory.ShowInfoPanel();
                    }
                }

            }
        }
        void OnTriggerEnter(Collider col)
        {

            if (col.CompareTag("Player"))
            {
                inventory.pickUpText.GetComponent<Text>().text = "Pick up\n" + this.name.ToUpper();
                inventory.pickUpText.SetActive(true);
                isTriggering = true;
            }
            else
            {

                isTriggering = false;
            }

        }
        void OnTriggerExit(Collider col)
        {
            inventory.pickUpText.SetActive(false);
            isTriggering = false;
        }
    }

}