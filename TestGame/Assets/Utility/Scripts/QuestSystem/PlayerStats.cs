using InventorySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuestSystem

{
    [Serializable]
    public class PlayerStats
    {
        public static PlayerStats instance;
        public int experience;
        public int maxExperience;
        public int minExperience;
        public int health;
        public int maxHealth;
        public int minHealth;
        public int level;
        public int coins;

        public Vector3 playerPosition;
        public float playerRotation;

        public Vector3 cameraPosition;
        public Vector3 cameraRotation;

        public List<Quest> activeListOfQuests;
        public List<Quest> availableListOfQuests;

        public List<Item> playerItems;
        public List<int> playerItemAmount;
    }
}
