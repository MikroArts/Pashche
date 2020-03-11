using QuestSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueTrigger : MonoBehaviour
    {
        public Dialogue dialogue;
        internal QuestManager questManager;
        void Start()
        {
            questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
            dialogue.npcName = name;
            StartCoroutine(Init());
            
        }
        IEnumerator Init()
        {
            yield return new WaitForSeconds(2);
            for (int i = 0; i < GetComponent<NPCController>().availableQuests.Count; i++)
            {
                foreach (var quest in questManager.listOfQuests)
                {
                    if (GetComponent<NPCController>().availableQuests[i] == quest.questId)
                    {
                        dialogue.quest = quest;
                    }
                }
            }
        }
        public void TriggerDialogue()
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);            
        }
    }
}
