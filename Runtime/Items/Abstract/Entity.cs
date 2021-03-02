using System;

namespace Packages.com.dehagge.inventorysystem.Runtime.Items.Abstract
{
    public abstract class Entity
    {
        public Guid Id = new Guid();
        public string Name;
        public string Description;
    }
}
