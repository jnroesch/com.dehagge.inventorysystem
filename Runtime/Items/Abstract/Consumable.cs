using UnityEngine;

namespace Packages.com.dehagge.inventorysystem.Runtime.Items.Abstract
{
    public abstract class Consumable : Item
    {
        public virtual void Consume()
        {
            Debug.Log($"consumed {Name}");
        }
    }
}