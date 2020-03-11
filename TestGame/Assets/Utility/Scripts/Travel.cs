using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Travel : MonoBehaviour
{
    bool talkNPC = false;
    public int travelCost;
    public GameObject travelPanel;
    public Image travelImage;

    Vector3 luke = new Vector3(406.24f, 21.3f, 290.5f);
    Vector3 pridvorica = new Vector3(88f, 35f, 289.5f);
    Vector3 kushicaHan = new Vector3(299.5f, 24f, 37.2f);
    void Update()
    {
        talkNPC = GameObject.Find("Player").GetComponent<RayCastPickUp>().interact;
        TravelPanelPrompt();
    }

    private void TravelPanelPrompt()
    {
        if (talkNPC && Input.GetButtonDown("Action") && GameObject.Find("Player").GetComponent<RayCastPickUp>().ColliderName == "Travel")
        {
            travelPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void TravelToLocation(int location)
    {
        if (GameObject.Find("Player").GetComponent<Player>().coin >= travelCost)
        {
            switch (location)
            {
                case 0:
                    StartCoroutine(TravelDellay());
                    GameObject.Find("Player").GetComponent<Player>().coin -= travelCost;
                    GameObject.Find("Player").GetComponent<Transform>().position = kushicaHan;
                    GameObject.Find("Player").GetComponent<Transform>().rotation = Quaternion.identity;
                    travelPanel.SetActive(false);
                    Time.timeScale = 1f;
                    break;
                case 1:
                    StartCoroutine(TravelDellay());
                    GameObject.Find("Player").GetComponent<Player>().coin -= travelCost;
                    GameObject.Find("Player").GetComponent<Transform>().position = pridvorica;
                    GameObject.Find("Player").GetComponent<Transform>().rotation = Quaternion.identity;
                    travelPanel.SetActive(false);
                    Time.timeScale = 1f;
                    break;
                case 2:
                    StartCoroutine(TravelDellay());
                    GameObject.Find("Player").GetComponent<Player>().coin -= travelCost;
                    GameObject.Find("Player").GetComponent<Transform>().position = luke;
                    GameObject.Find("Player").GetComponent<Transform>().rotation = Quaternion.identity;
                    travelPanel.SetActive(false);
                    Time.timeScale = 1f;
                    break;
                default:
                    break;
            }
        }
        else
        {
            InventorySystem.Inventory.instance.infoPanel.GetComponentInChildren<Text>().text = "You dont have enough money!";
            InventorySystem.Inventory.instance.infoPanel.GetComponent<Image>().color = Color.red;
            InventorySystem.Inventory.instance.ShowInfoPanel();
            travelPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        
    }

    IEnumerator TravelDellay()
    {
        travelImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        travelImage.gameObject.SetActive(false);
    }
}
