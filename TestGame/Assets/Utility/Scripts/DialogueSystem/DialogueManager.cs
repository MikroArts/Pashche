using QuestSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        private Queue<string> sentences;

        public Text dialogueText;
        public Text npcNameText;
        public Animator animatior;
        
        void Start()
        {  
            sentences = new Queue<string>();
        }
        public void StartDialogue(Dialogue dialogue)
        {            
            StartCoroutine(OpenDialogue());
            sentences.Clear();
            foreach (var quest in QuestManager.instance.listOfQuests)
            {
                if (quest.questId == dialogue.quest.questId)
                {
                    dialogue.quest = quest;
                    dialogue.sentences = quest.description.Split('\n');
                    foreach (var sentence in dialogue.sentences)
                    {
                        sentences.Enqueue(sentence);
                    }
                    DisplayNextSentence();
                }
            }
        }
        
        public void DisplayNextSentence()
        {            
            if (sentences.Count == 0)
            {               
                EndDialogue();
                return;
            }
            string sentence = sentences.Dequeue();

            npcNameText.text = sentence.Split(':')[0].ToUpper() + ":";            
            StartCoroutine(Type(sentence.Split(':')[1]));
        }
        public void EndDialogue()
        {
            StartCoroutine(CloseDialogue());
            Time.timeScale = 1f;            
        }




        IEnumerator OpenDialogue()
        {
            
            animatior.SetBool("IsOpen", true);            
            yield return new WaitForSeconds(.3f);
            
            Time.timeScale = 0f;
            InventorySystem.Inventory.instance.pickUpText.SetActive(false);
        }
        IEnumerator CloseDialogue()
        {
            animatior.SetBool("IsOpen", false);
            yield return new WaitForSeconds(.3f);            
            QuestManager.instance.questPanel.SetActive(true);
        }
        IEnumerator Type(string sentence)
        {
            GameObject.Find("ContinueDialogueButton").GetComponent<Button>().enabled = false;
            dialogueText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }
            GameObject.Find("ContinueDialogueButton").GetComponent<Button>().enabled = true;
        }
    }    
}
