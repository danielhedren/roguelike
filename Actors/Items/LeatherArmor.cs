using Microsoft.Xna.Framework;
using roguelike.Components;

namespace roguelike.Actors.Items
{
    public class LeatherArmor : Item
    {
        public LeatherArmor()
        {
            Components.Add(new ItemComponent
            {
                Name = "Leather Armor",
                Description = "The Breastplate and shoulder protectors of this armor are made of leather that has been stiffened by being boiled in oil.The rest of the armor is made of softer and more flexible materials.",
                Slot = ItemComponent.EquipmentSlot.Armor,
            });
            var entity = new EntityComponent(Color.White, Color.Transparent, ']');
            entity.IsWalkable = true;
            Components.Add(entity);
            Components.Add(new ArmorComponent
            {
                ArmorClass = 11,
                Modifier = StatsComponent.Stat.Dexterity,
                MaximumModifier = null
            });
        }
    }
}