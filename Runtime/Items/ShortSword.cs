using Packages.com.dehagge.inventorysystem.Runtime.Items.Abstract;
using UnityEngine;

namespace Packages.com.dehagge.inventorysystem.Runtime.Items
{
    public class ShortSword : CloseCombatWeapon
    {
        public ShortSword()
        {
            //Entity
            Name = "ShortSword";
            Description = "Size doesn't always matter";
            
            //Item
            Weight = 4;
            MaxStackSize = 1;
            CurrentStackSize = 1;
            Value = 20;
            Icon = Resources.Load<Sprite>($"Items/{nameof(ShortSword)}");
            
            //Weapon
            DamageType = DamageType.Slash;
            Damage = 3.5f;
        }
    }
}
