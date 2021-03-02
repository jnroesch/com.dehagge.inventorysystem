using System;
using System.Collections.Generic;
using Packages.com.dehagge.inventorysystem.Runtime.Items.Abstract;
using UnityEngine;

namespace Packages.com.dehagge.inventorysystem.Runtime
{
    public class InventoryDataChangedEventArgs
    {
        public (int, int) Slot;
    }

    public class InventoryData
    {
        //Weight
        public float MaxWeight;
        public float CurrentWeight => CalculateCurrentWeight();
        public bool Encumbered => CurrentWeight > MaxWeight;

        //Size
        public int SlotCountX;
        public int SlotCountY;
        public int Slots => SlotCountX * SlotCountY;

        public Item[,] Items;

        public event EventHandler<InventoryDataChangedEventArgs> OnInventoryDataChanged;

        public InventoryData(int slotCountX, int slotCountY, float maxWeight)
        {
            SlotCountX = slotCountX;
            SlotCountY = slotCountY;
            MaxWeight = maxWeight;
            Items = new Item[SlotCountX, SlotCountY];
        }

        private float CalculateCurrentWeight()
        {
            float accumulatedWeight = 0;

            for (var x = 0; x < SlotCountX; x++)
            {
                for (var y = 0; y < SlotCountY; y++)
                {
                    var currentItem = Items[x, y];
                    if (currentItem == null) continue;

                    accumulatedWeight += currentItem.Weight;
                }
            }

            return accumulatedWeight;
        }

        /// <summary>
        /// Tries to add the given Item(stack) to the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <returns>The remaining item if not everything could be added to the inventory</returns>
        public Item AddItemToInventory(Item item)
        {
            if (item == null || item.CurrentStackSize <= 0) return null;

            if (!item.IsStackable)
            {
                var (x, y) = GetFreeSlotInInventory();
                if (x < 0 || y < 0) return item;

                Items[x, y] = item;
                OnInventoryDataChanged?.Invoke(this, new InventoryDataChangedEventArgs {Slot = (x,y)});
            }
            else
            {
                //get all stacks of this item type
                var itemStacks = GetAllItemStacksForType(item);
                foreach (var itemStack in itemStacks)
                {
                    var remainingSpaceInStack = itemStack.GetRemainingStackSpace();
                    if (remainingSpaceInStack <= 0) continue;

                    var amountToMoveToStack = Mathf.Min(remainingSpaceInStack, item.CurrentStackSize);
                    itemStack.CurrentStackSize += amountToMoveToStack;
                    item.CurrentStackSize -= amountToMoveToStack;
                    OnInventoryDataChanged?.Invoke(this,
                        new InventoryDataChangedEventArgs {Slot = GetPositionOfItemInInventory(itemStack)});

                    //check if all items have been distributed
                    if (item.CurrentStackSize <= 0) return null;
                }

                //all existing stacks filled but there is still more inbound, so we create a new slot if possible
                var (x, y) = GetFreeSlotInInventory();
                if (x < 0 || y < 0) return item;
                Items[x, y] = item;
                OnInventoryDataChanged?.Invoke(this, new InventoryDataChangedEventArgs {Slot = (x,y)});
            }

            return null;
        }

        public void RemoveItemFromInventory(Item item)
        {
        }

        private (int x, int y) GetFreeSlotInInventory()
        {
            for (var y = 0; y < SlotCountY; y++)
            {
                for (var x = 0; x < SlotCountX; x++)
                {
                    var currentItem = Items[x, y];
                    if (currentItem != null) continue;

                    return (x, y);
                }
            }

            return (-1, -1);
        }

        private IEnumerable<Item> GetAllItemStacksForType(Item inputType)
        {
            if (inputType == null || !inputType.IsStackable) return new List<Item>();

            var type = inputType.GetType();
            var listOfItems = new List<Item>();

            for (var y = 0; y < SlotCountY; y++)
            {
                for (var x = 0; x < SlotCountX; x++)
                {
                    var currentItem = Items[x, y];
                    if (currentItem == null || currentItem.GetType() != type) continue;

                    listOfItems.Add(currentItem);
                }
            }

            return listOfItems;
        }

        private (int, int) GetPositionOfItemInInventory(Item item)
        {
            if (item == null) return (-1, -1);
            
            for (var y = 0; y < SlotCountY; y++)
            {
                for (var x = 0; x < SlotCountX; x++)
                {
                    var currentItem = Items[x, y];
                    if (currentItem == null || currentItem != item) continue;

                    return (x, y);
                }
            }

            return (-1, -1);
        }
    }
}