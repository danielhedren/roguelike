using Microsoft.Xna.Framework;
using roguelike.Components;

namespace roguelike.Actors.Items
{
    public class Dagger : Item
    {
        public Dagger()
        {
            Components.Add(new MeleeAttackComponent
            {
                Dice = 1,
                Sides = 4,
                Modifier = 0,
                Speed = 1
            });
            Components.Add(new ItemComponent
            {
                Name = "Dagger",
                Description = "Stab them with the pointy end",
                Slot = ItemComponent.EquipmentSlot.Weapon,
            });
            var entity = new EntityComponent(Color.White, Color.Transparent, '|');
            entity.IsWalkable = true;
            Components.Add(entity);
        }
    }
}