using UnityEngine;

namespace QuestSystem
{
    public class Goal
    {
        public GoalType goalType;
        public string goalDescription;
        public int currentAmount;
        public int requiredAmount;
        public string goalTag;
        public bool isComplete;
    }
    public enum GoalType
    {
        TriviaQuest,
        CollectionQuest,
        LocationQuest
    }
}