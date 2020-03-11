using System;
using System.Collections.Generic;
namespace QuestSystem
{
    [Serializable]
    public class Quest
    {
        public enum QuestProgress {notAvailable, available, accepted, completed, done}

        public int questId;
        public string title;
        public string description;

        public List<CollectionGoals> goals;
        public int nextQuest;
        public NPC sourceNPC;
        public NPC recieveNPC;

        public QuestProgress progress;

        public int experienceReward;
        public int coinReward;
    }
}
