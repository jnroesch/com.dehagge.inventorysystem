using UnityEngine;

namespace Packages.com.dehagge.inventorysystem.Runtime.Items.Abstract
{
    public abstract class Equipment : Item
    {
        public float MaxDurability;
        public float CurrentDurability;

        public virtual void Equip()
        {
            Debug.Log($"Equipping {Name}");
        }
        
        public virtual void UnEquip()
        {
            Debug.Log($"UnEquipping {Name}");
        }
    }
}