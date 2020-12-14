using Microsoft.Xna.Framework;
using roguelike.Components;
using roguelike.Utils;

namespace roguelike.Actors.Monsters
{
    public class GiantFireBeetle : Monster
    {
        public GiantFireBeetle()
        {
            var rand = new System.Random();

            Components.Add(new EntityComponent(Color.Red, Color.Transparent, 'b'));
            Components.Add(new HealthComponent(Random.Dice(1, 6, 1)));
            Components.Add(new MovementComponent());
            Components.Add(new MeleeAttackComponent {
                Dice = 1,
                Sides = 6,
                Modifier = -1,
                ToHit = 1,
                Speed = 1
            });
            Components.Add(new StatsComponent {
                Strength = 8,
                Dexterity = 10,
                Constitution = 12,
                Intelligence = 1,
                Wisdom = 7,
                Charisma = 3,
                HitDice = 6,
                ArmorClass = 13,
                ExperienceGained = 10
            });
            Components.Add(new NameComponent("Giant Fire Beetle"));
            Components.Add(new SimpleAIComponent());
        }
    }
}