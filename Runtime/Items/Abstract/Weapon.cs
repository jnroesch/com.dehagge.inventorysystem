namespace Packages.com.dehagge.inventorysystem.Runtime.Items.Abstract
{
    public enum DamageType
    {
        Slash,
        Pierce,
        Blunt
    }
    
    public abstract class Weapon : Equipment
    {
        public float Damage;
        public DamageType DamageType;
    }
}