using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class ItemModel
    {
        //public Item item;
        public int itemId;

        public string itemName;

        public string spriteName;
        public int itemPoints;
        public ItemType itemType;
        public ItemModel()
        {
            itemId = -1;
        }

        public enum ItemType
        {
            consumption_collectable,
            weapon_collectable,
            experience_collectable,
            unique_collectable
        }
    }
    
}
