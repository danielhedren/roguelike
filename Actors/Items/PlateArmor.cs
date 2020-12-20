using Microsoft.Xna.Framework;
using roguelike.Components;

namespace roguelike.Actors.Items
{
    public class PlateArmor : Item
    {
        public PlateArmor()
        {
            Components.Add(new ItemComponent
            {
                Name = "Plate Armor",
                Description = " Plate consists of shaped, interlocking metal plates to cover the entire body. A suit of plate includes gauntlets, heavy leather boots, a visored helmet, and thick layers of padding underneath the armor. Buckles and straps distribute the weight over the body.",
                Slot = ItemComponent.EquipmentSlot.Armor,
            });
            var entity = new EntityComponent(Color.White, Color.Transparent, ']');
            entity.IsWalkable = true;
            Components.Add(entity);
            Components.Add(new ArmorComponent
            {
                ArmorClass = 18,
                Modifier = StatsComponent.Stat.Strength,
                MaximumModifier = null
            });
        }
    }
}