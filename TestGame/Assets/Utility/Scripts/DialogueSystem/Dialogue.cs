using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [System.Serializable]
    public class Dialogue
    {
        public string npcName;
        public QuestSystem.Quest quest;
        [TextArea(3,10)]
        public string[] sentences;

        public string NpcName { get; set; }
        public QuestSystem.Quest Quest { get; set; }
        public string[] Sentences { get; set; }
    }    
}
