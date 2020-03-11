using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using UnityEngine.UI;
using System.Linq;
using System;
using DialogueSystem;

public class NPCController : MonoBehaviour
{
    public List<int> availableQuests = new List<int>();
    public List<int> receivableQuests = new List<int>();
    public bool isTriggering = false;

    //test sprite
    public GameObject question;
    public GameObject exclamation;
    public Text npcName;

    bool talkNPC;
    void Start()
    {
        StartCoroutine(FillNPCListOfAvailableQuests());
        npcName.text = name;
    }

    IEnumerator FillNPCListOfAvailableQuests()
    {
        yield return new WaitForSeconds(1f);
        foreach (var q in QuestManager.instance.listOfQuests.ToArray())
        {
            if (q.progress == Quest.QuestProgress.available && q.sourceNPC.NpcName == transform.name)
            {
                availableQuests.Add(q.questId);
            }
        }       
    }

    void Update()
    {        
        CheckForQuestState();
        talkNPC = GameObject.Find("Player").GetComponent<RayCastPickUp>().interact;
        if (QuestManager.instance.playerListOfQuests.Count > 0)
        {
            for (int i = 0; i < QuestManager.instance.playerListOfQuests.Count; i++)
            {
                if (QuestManager.instance.playerListOfQuests[i].progress == Quest.QuestProgress.completed && QuestManager.instance.playerListOfQuests[i].recieveNPC.NpcName == transform.name)
                {
                    exclamation.SetActive(false);
                    question.SetActive(true);
                    return;
                }
                else
                {
                    question.SetActive(false);
                }
            }
        }
        if (availableQuests.Count != 0)
        {
            for (int i = 0; i < QuestManager.instance.listOfQuests.Count; i++)
            {
                if (QuestManager.instance.listOfQuests[i].progress == Quest.QuestProgress.available && QuestManager.instance.listOfQuests[i].sourceNPC.NpcName == transform.name)
                {
                    exclamation.SetActive(true);
                    return;
                }
                else
                {
                    exclamation.SetActive(false);
                }
            }
        }

    }

    public void CheckForQuestState()
    {
        if (talkNPC && Input.GetButtonDown("Action"))
        {
            
            if (GameObject.Find("Player").GetComponent<RayCastPickUp>().ColliderName == transform.name)
            {
                gameObject.transform.LookAt(GameObject.Find("Player").transform);
                CompletingQuest();
            }
        }
    }

    public void CompletingQuest()
    {        
        foreach (var quest in QuestManager.instance.playerListOfQuests.ToArray())
        {
            if (quest.progress == Quest.QuestProgress.completed && transform.name == quest.recieveNPC.NpcName)
            {
                receivableQuests.Add(quest.questId);
                QuestManager.instance.CompleteQuest(quest.questId, quest.sourceNPC.NpcName);
                question.SetActive(false);

                QuestManager.instance.questCompleted.text = "\"" + quest.title + "\"\nQuest Completed";
                QuestManager.instance.gotCoinText.text = quest.coinReward.ToString();
                QuestManager.instance.gotXPText.text = quest.experienceReward.ToString();
                QuestManager.instance.haveCoinText.text = QuestManager.instance.player.GetComponent<Player>().coin.ToString();
                QuestManager.instance.haveXPText.text = QuestManager.instance.player.GetComponent<Player>().experience.ToString();
                QuestManager.instance.needXPText.text = (QuestManager.instance.player.GetComponent<Player>().experienceFill.maxValue - int.Parse(QuestManager.instance.haveXPText.text)).ToString();

                QuestManager.instance.finishQuestPanel.SetActive(true);

                return;
            }
        }
        GiveNewQuestFromNPC();

    }

    private void GiveNewQuestFromNPC()
    {        
        foreach (var quest in QuestManager.instance.listOfQuests)
        {
            if (quest.progress == Quest.QuestProgress.available && quest.sourceNPC.NpcName == GameObject.Find("Player").GetComponent<RayCastPickUp>().ColliderName)
            {
                if (QuestManager.instance.playerListOfQuests.Count != 0)
                {
                    foreach (var q in QuestManager.instance.playerListOfQuests)
                    {
                        if ((q.progress == Quest.QuestProgress.accepted || q.progress == Quest.QuestProgress.completed) && q.sourceNPC.NpcName == name)
                        {
                            return;
                        }
                    }
                }
                
                GetComponent<DialogueTrigger>().TriggerDialogue();
                QuestManager.instance.TriggerNpcQuest(this, quest.questId);
                return;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            isTriggering = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            isTriggering = false;
        }
    }
    public bool HaveInteraction()
    {
        if (exclamation.activeSelf || question.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}