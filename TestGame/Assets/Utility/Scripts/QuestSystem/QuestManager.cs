using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
//using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using System;
using InventorySystem;

//TODO: Document all methods
namespace QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager instance;
        private string path = "https://mikroarts.com//DidYouSeeMyPashche//StreamingAssets//quests.json";
        public List<Quest> listOfQuests;
        public List<Quest> playerListOfQuests;
        public GameObject player;
        Inventory inventory;

        /* - Quest UI elements - */
        public GameObject questPanel;
        
        public Text currentActiveQuests;
        public Text questTitle;
        private string[] qGoal;
        public Text questGoals;
        public Text reqAndCurrAmmount;
        public Text questExperienceReward;
        public Text questCoinsReward;
        public Text qId;

        public GameObject finishQuestPanel;
        public Button finishQUestButton;
        public Text gotXPText;
        public Text haveXPText;
        public Text gotCoinText;
        public Text haveCoinText;
        public Text needXPText;
        public Text questCompleted;

        public bool canShowQuest;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            //DontDestroyOnLoad(gameObject);

            //path = Application.streamingAssetsPath + "/Quests.json";
            //jsonString = File.ReadAllText(path);
            //listOfQuests = ReturnAllQuests();
            
        }
        void Start()
        {
            canShowQuest = false;
            player = GameObject.FindGameObjectWithTag("Player");
            inventory = GameObject.Find("InventoryManager").GetComponent<Inventory>();

            questPanel.SetActive(false);
            finishQuestPanel.SetActive(false);            
            StartCoroutine(GetRequest(path));
        }
        IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else
                {
                    listOfQuests = JsonConvert.DeserializeObject<List<Quest>>(webRequest.downloadHandler.text);
                }
            }
        }
        void Update()
        {
            CheckGoals();
            QuestPanelPrompt();
            FinishQuestPanelPrompt();
        }

        public void FinishQuestPanelPrompt()
        {
            if (finishQuestPanel.activeSelf)
            {
                Time.timeScale = 0f;
            }
        }
        public void CloseFinishQuestPanel()
        {
            finishQuestPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        
        public void AcceptQuest(int questId)
        {
            for (int i = 0; i < listOfQuests.Count; i++)
            {
                if (listOfQuests[i].questId == questId && listOfQuests[i].progress == Quest.QuestProgress.available)
                {
                    playerListOfQuests.Add(listOfQuests[i]);
                    listOfQuests[i].progress = Quest.QuestProgress.accepted;
                    inventory.infoPanel.GetComponentInChildren<Text>().text = "Quest accepted!";
                    inventory.infoPanel.GetComponent<Image>().color = Color.green;
                    inventory.ShowInfoPanel();                    
                }
            }
        }
        
        public void QuitQuest(int questId)
        {
            for (int i = 0; i < playerListOfQuests.Count; i++)
            {
                if (playerListOfQuests[i].questId == questId && playerListOfQuests[i].progress == Quest.QuestProgress.accepted)
                {
                    playerListOfQuests[i].progress = Quest.QuestProgress.available;
                    playerListOfQuests.Remove(playerListOfQuests[i]);
                }
            }
        }
        
        public void CompleteQuest(int questId, string sourceNpcName)
        {
            for (int i = 0; i < playerListOfQuests.Count; i++)
            {
                if (playerListOfQuests[i].questId == questId && playerListOfQuests[i].progress == Quest.QuestProgress.completed)
                {                    
                    playerListOfQuests[i].progress = Quest.QuestProgress.done;                    
                    RemoveCollectedQuestItems(questId);
                    player.GetComponent<Player>().FillPlayerData(playerListOfQuests[i].experienceReward, playerListOfQuests[i].coinReward);
                    playerListOfQuests.Remove(playerListOfQuests[i]);

                    foreach (var q in listOfQuests)
                    {
                        if (q.progress == Quest.QuestProgress.notAvailable && q.sourceNPC.NpcName == sourceNpcName)
                        {
                            q.progress = Quest.QuestProgress.available;
                            return;
                        }
                    }
                }
            }
        }

        private void RemoveCollectedQuestItems(int id)
        {
            foreach (var quest in playerListOfQuests)
            {
                if (quest.questId == id)
                {
                    for (var j = 0; j < quest.goals.Count; j++)
                    {
                        if (inventory.items[j].itemId > -1 && quest.goals[j].goalType == GoalType.CollectionQuest && quest.goals[j].goalTag == inventory.items.Find(p => p.itemName == quest.goals[j].goalTag).itemName)
                        {
                            foreach (var slot in inventory.slots)
                            {
                                if (slot.name == quest.goals[j].goalTag)
                                {
                                    slot.GetComponentInChildren<Text>().text = (int.Parse(slot.GetComponentInChildren<Text>().text) - quest.goals[j].requiredAmount).ToString();

                                    if (int.Parse(slot.GetComponentInChildren<Text>().text) <= 0)
                                    {
                                        inventory.items.Remove(inventory.database.ReturnItemByName(quest.goals[j].goalTag));
                                        for (int i = 0; i < inventory.slotAmount; i++)
                                        {
                                            if (inventory.slotPanel.transform.GetChild(i).name == quest.goals[j].goalTag)
                                            {
                                                Destroy(inventory.slotPanel.transform.GetChild(i).gameObject);
                                                inventory.slots.Remove(inventory.slots[i]);
                                                inventory.items.Add(new ItemModel());
                                                GameObject go = Instantiate(inventory.inventorySlot);
                                                inventory.slots.Add(go);
                                                go.transform.SetParent(inventory.slotPanel.transform);
                                                go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                                            }
                                        }
                                    }
                                }
                            }
                            return;
                        }
                    }
                }
                else
                    return;
            }
            
        }
        public void CheckGoals()
        {
            for (int i = 0; i < playerListOfQuests.Count; i++)
            {
                for (int j = 0; j < playerListOfQuests[i].goals.Count; j++)
                {
                    if (playerListOfQuests[i].goals[j].currentAmount == playerListOfQuests[i].goals[j].requiredAmount)
                    {
                        playerListOfQuests[i].goals[j].isComplete = true;
                    }
                }

                if (playerListOfQuests[i].goals.All(p => p.isComplete) && playerListOfQuests[i].progress != Quest.QuestProgress.completed)
                {
                    playerListOfQuests[i].progress = Quest.QuestProgress.completed;                    
                    inventory.infoPanel.GetComponentInChildren<Text>().text = "Quest complete!\nTalk to " + playerListOfQuests[i].recieveNPC.NpcName;
                    inventory.infoPanel.GetComponent<Image>().color = Color.cyan;
                    inventory.ShowInfoPanel();
                }
            }
            foreach (Quest quest in playerListOfQuests)
            {
                for (int i = 0; i < quest.goals.Count; i++)
                {
                    foreach (var item in inventory.items)
                    {
                        if (item.itemName == quest.goals[i].goalTag && quest.goals[i].currentAmount >= quest.goals[i].requiredAmount && int.Parse(inventory.itemAmountText.text) >= quest.goals[i].requiredAmount)
                        {
                            quest.goals[i].isComplete = true;
                            UpdateQuestInfo();
                            return;
                        }
                        else if (item.itemName == quest.goals[i].goalTag && quest.goals[i].currentAmount < quest.goals[i].requiredAmount)
                        {
                            quest.goals[i].currentAmount++;
                            quest.goals[i].currentAmount = int.Parse(inventory.itemAmountText.text);
                        }
                    }
                }
            }
            UpdateQuestInfo();

        }

        private void QuestPanelPrompt()
        {
            if (questPanel.activeSelf)
            {
                Time.timeScale = 0f;
            }
        }

        public void TriggerNpcQuest(NPCController npcController, int id)
        {
            ClearAllTexts();
            Quest q = listOfQuests.Find(p => p.questId == id);

            // Activate GUI prompt            
            FillQuestInfo(q);
            //questPanel.SetActive(true);
        }

        private void FillQuestInfo(Quest q)
        {
            qId.text = q.questId.ToString();
            questTitle.text = q.title;
            questCoinsReward.text = q.coinReward.ToString();
            questExperienceReward.text = q.experienceReward.ToString();
            foreach (var goal in q.goals)
            {
                reqAndCurrAmmount.text += goal.currentAmount + "/" + goal.requiredAmount + "\n";
            }
            foreach (var goal in q.goals)
            {
                questGoals.text += "- " + goal.goalDescription + "\n";
            }
        }

        public void GiveTransformTag(string tag)
        {
            foreach (Quest quest in playerListOfQuests)
            {
                if (quest.goals.Count != 0)
                {
                    for (int i = 0; i < quest.goals.Count; i++)
                    {
                        if (quest.goals[i].goalTag == tag && quest.goals[i].currentAmount < quest.goals[i].requiredAmount)
                        {
                            quest.goals[i].currentAmount++;
                        }                        
                    }
                }
            }
        }

        

        public void UpdateQuestInfo()
        {
            currentActiveQuests.text = "";
            if (playerListOfQuests.Count > 0)
            {
                foreach (var quest in playerListOfQuests)
                {
                    currentActiveQuests.text += quest.title + ": \t" + "<b>" + quest.progress + "</b>" + "\n";
                    for (int i = 0; i < quest.goals.Count; i++)
                    {
                        currentActiveQuests.text += quest.goals[i].goalDescription + ": " + quest.goals[i].currentAmount + "/" + quest.goals[i].requiredAmount + "\n";
                    }                    
                    currentActiveQuests.text += "\n";
                }
            }
            else
            {
                currentActiveQuests.text = "You don't have active quests!";
            }
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                                                                  //
        //                                           Methods to work with 'database/JSON' files                                             //
        //                                                                                                                                  //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void SetQuest()
        {
            Quest q = new Quest();
            q.questId = 1;
            q.title = "First quest";
            q.description = "Description for first quest";
            q.goals = new List<CollectionGoals>();
            q.goals.Add(new CollectionGoals { currentAmount = 0, requiredAmount = 1, isComplete = false });
            q.sourceNPC = new NPC();
            q.sourceNPC.NpcName = "testNPC_1";
            q.experienceReward = 10;
            q.coinReward = 50;
            //listOfQuests.Add(q);
            string s = JsonConvert.SerializeObject(q);
            File.WriteAllText(path, s);
        }
        void ClearAllTexts()
        {
            questTitle.text = "";
            questGoals.text = "";
            reqAndCurrAmmount.text = "";
            questExperienceReward.text = "";
            questCoinsReward.text = "";
            qId.text = "";
        }
    }
}