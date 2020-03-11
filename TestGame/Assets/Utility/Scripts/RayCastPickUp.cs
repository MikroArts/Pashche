using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCastPickUp : MonoBehaviour
{
    public new Camera camera;
    Inventory inventory;
    public float targetDistance = 100f;
    public float minColliderDist = 6.5f;
    public float yView = .1f;

    public bool interact;
    public string ColliderName;
    void Start()
    {
        inventory = GameObject.Find("InventoryManager").GetComponent<Inventory>();
    }

    void Update()
    {
        CheckIfColliderIsHit();
    }

    private void CheckIfColliderIsHit()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward + new Vector3(0, yView, 0), out hit, targetDistance) && Time.timeScale > 0)
        {
            Debug.DrawLine(camera.transform.position, hit.point, Color.magenta);
            if (hit.distance < minColliderDist)
            {
                ColliderName = hit.collider.name;
                if (hit.collider.CompareTag("Collectible"))
                {
                    inventory.pickUpText.GetComponent<Text>().text = "Pick up\n" + hit.collider.name.ToUpper();
                    inventory.pickUpText.SetActive(true);
                    hit.collider.GetComponent<PickUp>().PickUpPrompt();
                }
                else if (hit.collider.CompareTag("NPC") || hit.collider.CompareTag("Milivoje"))
                {
                    
                    QuestSystem.QuestManager.instance.GiveTransformTag(hit.collider.tag);
                    

                    if (hit.collider.GetComponent<NPCController>().HaveInteraction())
                    {
                        interact = true;
                        if (ColliderName.StartsWith("Quest"))
                            inventory.pickUpText.GetComponent<Text>().text = "[E]\nRead " + ColliderName;
                        else
                            inventory.pickUpText.GetComponent<Text>().text = "[E]\nTalk to " + ColliderName;
                        inventory.pickUpText.SetActive(true);
                    }
                    else
                    {
                        interact = false;
                        inventory.pickUpText.SetActive(false);
                    }
                }
                else
                {                    
                    switch (hit.collider.tag)
                    {
                        case "Coin":
                            inventory.pickUpText.GetComponent<Text>().text = "Pick up\n" + hit.collider.name.ToUpper();
                            inventory.pickUpText.SetActive(true);
                            hit.collider.GetComponent<PickUp>().PickUpPrompt();
                            break;
                        case "CoinBag":
                            inventory.pickUpText.GetComponent<Text>().text = "Pick up\n" + hit.collider.name.ToUpper();
                            inventory.pickUpText.SetActive(true);
                            hit.collider.GetComponent<PickUp>().PickUpPrompt();
                            break;
                        case "CoinPile":
                            inventory.pickUpText.GetComponent<Text>().text = "Pick up\n" + hit.collider.name.ToUpper();
                            inventory.pickUpText.SetActive(true);
                            hit.collider.GetComponent<PickUp>().PickUpPrompt();
                            break;
                        case "CoinChest":
                            inventory.pickUpText.GetComponent<Text>().text = "Pick up\n" + hit.collider.name.ToUpper();
                            inventory.pickUpText.SetActive(true);
                            hit.collider.GetComponent<PickUp>().PickUpPrompt();
                            break;
                        case "Sign":
                            inventory.pickUpText.GetComponent<Text>().text = hit.collider.name;
                            inventory.pickUpText.SetActive(true);
                            break;
                        case "Travel":
                            inventory.pickUpText.GetComponent<Text>().text = hit.collider.name;
                            inventory.pickUpText.SetActive(true);
                            interact = true;
                            break;
                        case "Chest":
                            inventory.pickUpText.GetComponent<Text>().text = (hit.collider.GetComponent<Chest>().isOpen) ? ("Close\n" + hit.collider.name) : ("Open\n" + hit.collider.name);
                            inventory.pickUpText.SetActive(true);
                            hit.collider.GetComponent<Chest>().OpenCloseChest(hit.collider.GetComponent<Chest>().isOpen);
                            interact = true;
                            break;
                        default:
                            inventory.pickUpText.SetActive(false);
                            break;
                    }
                }
            }
            else
            {
                interact = false;
                inventory.pickUpText.SetActive(false);
            }
                
        }
    }
    
}
