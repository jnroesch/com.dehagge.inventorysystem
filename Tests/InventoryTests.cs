using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Packages.com.dehagge.inventorysystem.Runtime;
using Packages.com.dehagge.inventorysystem.Runtime.Items;
using Packages.com.dehagge.inventorysystem.Runtime.Items.Abstract;
using Packages.com.DeHagge.PrivateTestFramework.Runtime;

namespace Packages.com.dehagge.inventorysystem.Tests
{
    [TestFixture]
    public class CalculateCurrentWeight
    {
        [Test]
        public void EmptyInventory_ThenReturnZero()
        {
            var inventory = new InventoryData(1, 1, 10);
            Assert.AreEqual(0, inventory.CurrentWeight);
        }

        [Test]
        public void OneItemInInventory_ThenReturnItemWeight()
        {
            var inventory = new InventoryData(1, 1, 10);
            var item = new ShortSword();
            inventory.Items[0, 0] = item;
            Assert.AreEqual(item.Weight, inventory.CurrentWeight);
        }

        [Test]
        public void OneItemStackInInventory_ThenReturnItemStackWeight()
        {
            var inventory = new InventoryData(1, 1, 10);
            const int stackSize = 10;

            var item = new SmallHealthPotion {CurrentStackSize = stackSize};
            inventory.Items[0, 0] = item;
            Assert.AreEqual(item.Weight, inventory.CurrentWeight);
        }

        [Test]
        public void MultipleSingleItemsInInventory_ThenReturnTotalWeight()
        {
            var inventory = new InventoryData(2, 1, 10);
            var item1 = new ShortSword();
            var item2 = new ShortSword();
            inventory.Items[0, 0] = item1;
            inventory.Items[1, 0] = item2;

            Assert.AreEqual(item1.Weight + item2.Weight, inventory.CurrentWeight);
        }

        [Test]
        public void MultipleStackedItemsInInventory_ThenReturnTotalWeight()
        {
            var inventory = new InventoryData(2, 1, 10);
            var item1 = new SmallHealthPotion {CurrentStackSize = 5};
            var item2 = new SmallHealthPotion {CurrentStackSize = 3};
            inventory.Items[0, 0] = item1;
            inventory.Items[1, 0] = item2;

            Assert.AreEqual(item1.Weight + item2.Weight, inventory.CurrentWeight);
        }

        [Test]
        public void MultipleItemsInInventory_ThenReturnTotalWeight()
        {
            var inventory = new InventoryData(3, 1, 10);
            var item1 = new ShortSword();
            var item2 = new SmallHealthPotion {CurrentStackSize = 3};
            inventory.Items[0, 0] = item1;
            inventory.Items[1, 0] = item2;

            Assert.AreEqual(item1.Weight + item2.Weight, inventory.CurrentWeight);
        }
    }

    public class Encumbered
    {
        [Test]
        public void InventoryIsEmpty_ThenFalse()
        {
            var inventory = new InventoryData(3, 1, 10);
            Assert.IsFalse(inventory.Encumbered);
        }

        [Test]
        public void InventoryNotFull_ThenFalse()
        {
            var inventory = new InventoryData(3, 1, 10);
            var item = new ShortSword();
            inventory.Items[0, 0] = item;

            Assert.IsFalse(inventory.Encumbered);
        }

        [Test]
        public void InventoryFull_ThenFalse()
        {
            var inventory = new InventoryData(3, 1, 8);
            var item1 = new ShortSword();
            var item2 = new ShortSword();
            inventory.Items[0, 0] = item1;
            inventory.Items[1, 0] = item2;

            Assert.IsFalse(inventory.Encumbered);
        }

        [Test]
        public void InventoryOverFull_ThenFalse()
        {
            var inventory = new InventoryData(3, 1, 5);
            var item1 = new ShortSword();
            var item2 = new ShortSword();
            inventory.Items[0, 0] = item1;
            inventory.Items[1, 0] = item2;

            Assert.IsTrue(inventory.Encumbered);
        }

        [Test]
        public void ZeroInventoryMaxWeight_ThenFalse()
        {
            var inventory = new InventoryData(0, 0, 0);

            Assert.IsFalse(inventory.Encumbered);
        }
    }

    public class AddItemToInventory
    {
        [Test]
        public void InventoryHasNoSlots_ThenReturnItem()
        {
            var inventory = new InventoryData(0, 0, 0);
            var item = new ShortSword();

            var remainingItem = inventory.AddItemToInventory(item);
            Assert.AreEqual(item, remainingItem);
        }

        [Test]
        public void InventoryFreeSlots_ThenReturnNull()
        {
            var inventory = new InventoryData(1, 1, 0);
            var item = new ShortSword();

            var remainingItem = inventory.AddItemToInventory(item);
            Assert.AreEqual(null, remainingItem);
        }

        [Test]
        public void ItemIsNull_ThenReturnNull()
        {
            var inventory = new InventoryData(1, 1, 0);

            Assert.AreEqual(null, inventory.AddItemToInventory(null));
        }

        [Test]
        public void StackPerfectlyFits_ThenReturnNull()
        {
            var inventory = new InventoryData(1, 1, 0);
            var item1 = new SmallHealthPotion {CurrentStackSize = 10};
            inventory.Items[0, 0] = item1;

            var item2 = new SmallHealthPotion {CurrentStackSize = 10};

            Assert.AreEqual(null, inventory.AddItemToInventory(item2));
        }

        [Test]
        public void StackDoesNotFit_ThenReturnRemainingStack()
        {
            var inventory = new InventoryData(1, 1, 0);
            var item1 = new SmallHealthPotion {CurrentStackSize = 13};
            inventory.Items[0, 0] = item1;

            var item2 = new SmallHealthPotion {CurrentStackSize = 10};

            var remainingItems = inventory.AddItemToInventory(item2);
            Assert.AreEqual(3, remainingItems.CurrentStackSize);
        }

        [Test]
        public void StackDoesNotFitButSpaceAvailable_ThenReturnNull()
        {
            var inventory = new InventoryData(2, 1, 0);
            var item1 = new SmallHealthPotion {CurrentStackSize = 13};
            inventory.Items[0, 0] = item1;

            var item2 = new SmallHealthPotion {CurrentStackSize = 10};

            var remainingItems = inventory.AddItemToInventory(item2);
            Assert.IsNull(remainingItems);
            Assert.IsNotNull(inventory.Items[1, 0]);
            Assert.AreEqual(20, inventory.Items[0, 0].CurrentStackSize);
            Assert.AreEqual(3, inventory.Items[1, 0].CurrentStackSize);
        }
    }

    public class GetFreeSlotInInventory : TestFixture
    {
        [Test]
        public void NoSlotsAvailable_ThenReturnInvalid()
        {
            var inventory = new InventoryData(0, 0, 1);
            Assert.AreEqual((-1, -1), InvokePrivateMethod<(int, int)>(inventory, "GetFreeSlotInInventory"));
        }

        [Test]
        public void OneAvailable_ThenReturnSlot()
        {
            var inventory = new InventoryData(1, 1, 1);
            Assert.AreEqual((0, 0), InvokePrivateMethod<(int, int)>(inventory, "GetFreeSlotInInventory"));
        }

        [Test]
        public void AllSlotsFull_ThenReturnInvalid()
        {
            var inventory = new InventoryData(1, 1, 1) {Items = {[0, 0] = new ShortSword()}};
            Assert.AreEqual((-1, -1), InvokePrivateMethod<(int, int)>(inventory, "GetFreeSlotInInventory"));
        }

        [Test]
        public void MultipleFreeSlots_ThenReturnFirstSlot()
        {
            var inventory = new InventoryData(5, 1, 1) {Items = {[0, 0] = new ShortSword(), [2, 0] = new ShortSword()}};
            Assert.AreEqual((1, 0), InvokePrivateMethod<(int, int)>(inventory, "GetFreeSlotInInventory"));
        }
    }

    public class GetAllItemStacksForType : TestFixture
    {
        [Test]
        public void ItemIsNull_ThenReturnEmptyList()
        {
            var inventory = new InventoryData(5, 1, 1);
            Item item = null;
            Assert.IsEmpty(InvokePrivateMethod<IEnumerable<Item>>(inventory, "GetAllItemStacksForType", item));
        }

        [Test]
        public void NoItemsInInventory_ThenReturnEmptyList()
        {
            var inventory = new InventoryData(5, 1, 1);
            Assert.IsEmpty(InvokePrivateMethod<IEnumerable<Item>>(inventory, "GetAllItemStacksForType",
                new SmallHealthPotion()));
        }

        [Test]
        public void ItemIsNotStackable_ThenReturnEmptyList()
        {
            var inventory = new InventoryData(5, 1, 1);
            Assert.IsEmpty(
                InvokePrivateMethod<IEnumerable<Item>>(inventory, "GetAllItemStacksForType", new ShortSword()));
        }

        [Test]
        public void SingleStackExists_ThenReturnStack()
        {
            var itemInInventory = new SmallHealthPotion();
            var inventory = new InventoryData(5, 1, 1) {Items = {[0, 0] = itemInInventory}};
            var itemToCheck = new SmallHealthPotion();
            var result =
                InvokePrivateMethod<IEnumerable<Item>>(inventory, "GetAllItemStacksForType", itemToCheck);
            Assert.IsNotEmpty(result);
            Assert.Contains(itemInInventory, result.ToList());
            Assert.IsFalse(result.Contains(itemToCheck));
        }

        [Test]
        public void DifferentItemsExist_ThenReturnStack()
        {
            var itemInInventory = new SmallHealthPotion();
            var swordInInventory = new ShortSword();
            var inventory = new InventoryData(5, 1, 1) {Items = {[0, 0] = itemInInventory, [1, 0] = swordInInventory}};
            var itemToCheck = new SmallHealthPotion();
            var result =
                InvokePrivateMethod<IEnumerable<Item>>(inventory, "GetAllItemStacksForType", itemToCheck);
            Assert.IsNotEmpty(result);
            Assert.Contains(itemInInventory, result.ToList());
            Assert.IsFalse(result.Contains(itemToCheck));
            Assert.IsFalse(result.Contains(swordInInventory));
        }

        [Test]
        public void MultipleStacksOfItemExist_ThenReturnAllStacks()
        {
            var stack1InInventory = new SmallHealthPotion();
            var stack2InInventory = new SmallHealthPotion();
            var inventory = new InventoryData(5, 1, 1)
            {
                Items = {[0, 0] = stack1InInventory, [1, 0] = stack2InInventory}
            };
            var itemToCheck = new SmallHealthPotion();
            var result =
                InvokePrivateMethod<IEnumerable<Item>>(inventory, "GetAllItemStacksForType", itemToCheck);
            Assert.IsNotEmpty(result);
            Assert.Contains(stack1InInventory, result.ToList());
            Assert.Contains(stack2InInventory, result.ToList());
            Assert.IsFalse(result.Contains(itemToCheck));
        }
    }
}