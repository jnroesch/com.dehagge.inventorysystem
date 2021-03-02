using UnityEngine;

namespace Packages.com.dehagge.inventorysystem.Runtime.Items.Abstract
{
    public abstract class Item : Entity
    {
        public float Weight
        {
            get => GetTotalWeight();
            protected set => _weight = value;
        }

        private float _weight;
        
        public int MaxStackSize;
        public int CurrentStackSize;
        public bool IsStackable => MaxStackSize > 1;
        public float Value;
        public Sprite Icon;

        public int GetRemainingStackSpace()
        {
            if (!IsStackable) return 0;

            return MaxStackSize - CurrentStackSize;
        }

        public float GetTotalValue()
        {
            if (!IsStackable) return Value;

            return Value * CurrentStackSize;
        }

        private float GetTotalWeight()
        {
            if (!IsStackable) return _weight;

            return _weight * CurrentStackSize;
        }
    }
}