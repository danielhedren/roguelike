using System.Collections.Generic;
using roguelike.Actors.Items;

namespace roguelike.Components
{
    public class InventoryComponent : Component
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Item> EquippedItems { get; set; } = new List<Item>();
        public InventoryComponent()
        {

        }
    }
}