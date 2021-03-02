namespace Packages.com.dehagge.inventorysystem.Runtime.Items.Abstract
{
    public enum ArmorType
    {
        Light,
        Medium,
        Heavy
    }
    
    public abstract class Armor : Equipment
    {
        public float PhysicalArmor;
        public float MagicalArmor;
        public ArmorType ArmorType;
    }
}