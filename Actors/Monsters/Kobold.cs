using Microsoft.Xna.Framework;
using roguelike.Components;

namespace roguelike.Actors.Monsters
{
    public class Kobold : Monster
    {
        public Kobold()
        {
            var rand = new System.Random();

            Components.Add(new EntityComponent(Color.Brown, Color.Transparent, 'k'));
            Components.Add(new HealthComponent(Utils.Roll(2, 6, -2)));
            Components.Add(new MovementComponent());
            Components.Add(new MeleeAttackComponent {
                Dice = 1,
                Sides = 4,
                Modifier = 2,
                ToHit = 4,
                Speed = 1
            });
            Components.Add(new StatsComponent {
                Strength = 7,
                Dexterity = 15,
                Constitution = 9,
                Intelligence = 8,
                Wisdom = 7,
                Charisma = 8,
                HitDice = 6,
                ArmorClass = 12,
                ExperienceGained = 25
            });
            Components.Add(new NameComponent("Kobold"));
            Components.Add(new SimpleAIComponent());
        }
    }
}