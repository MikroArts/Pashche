using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "item")]
    public class Item : ScriptableObject
    {
        
        public int itemId;

        public string itemName;

        public string spriteName;
        public int itemPoints;
        public ItemType itemType;

        public enum ItemType
        {
            consumption_collectable,
            weapon_collectable,
            experience_collectable,
            unique_collectable
        }
    }
}
