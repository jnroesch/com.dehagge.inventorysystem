using Packages.com.dehagge.inventorysystem.Runtime.Items.Abstract;

namespace Packages.com.dehagge.inventorysystem.Runtime.Items
{
    public class SmallHealthPotion : Consumable
    {
        public SmallHealthPotion()
        {
            //Entity
            Name = "Small Health Potion";
            Description = "Keeps you alive just a little bit longer";
            
            //Item
            Weight = 0.1f;
            MaxStackSize = 20;
            CurrentStackSize = 1;
            Value = 3;
        }
    }
}
