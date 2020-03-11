using InventorySystem;
using QuestSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestTracker : MonoBehaviour
{
    Inventory inventory;
    QuestManager questManager;
    // Use this for initialization
    void Start() {
        inventory = GameObject.Find("InventoryManager").GetComponent<Inventory>();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }
    public void OnTriggerEnter()
    {        
        QuestManager.instance.GiveTransformTag(transform.tag);        
        foreach (var quest in questManager.playerListOfQuests)
        {              
            for (int i = 0; i < quest.goals.Count; i++)
            {
                
                if (quest.goals[i].goalTag == transform.tag && quest.goals[i].currentAmount == quest.goals[i].requiredAmount)
                {
                    if (quest.goals[i].goalTag == transform.tag && quest.goals[i].isComplete == true)
                    {
                        return;
                    }
                    inventory.infoPanel.GetComponentInChildren<Text>().text = "Goal complete!";
                    inventory.infoPanel.GetComponent<Image>().color = Color.green;
                    inventory.ShowInfoPanel();                    
                }                
            }            
        }
        questManager.UpdateQuestInfo();
    }
}
