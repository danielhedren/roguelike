using System.Collections.Generic;
using roguelike.Actors.Items;

namespace roguelike.Components
{
    public class InventoryComponent : Component
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public Dictionary<ItemComponent.EquipmentSlot, Item> EquipmentSlots { get; set; } = new Dictionary<ItemComponent.EquipmentSlot, Item> {
            { ItemComponent.EquipmentSlot.Weapon, null },
            { ItemComponent.EquipmentSlot.Armor, null },
        };
        public InventoryComponent()
        {

        }
    }
}