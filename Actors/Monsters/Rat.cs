using Microsoft.Xna.Framework;
using roguelike.Components;
using roguelike.Utils;

namespace roguelike.Actors.Monsters
{
    public class Rat : Monster
    {
        private static Color[] _colors = new Color[] { Color.Brown, Color.White, Color.Goldenrod, Color.LightGray };
        public Rat()
        {
            var rand = new System.Random();

            Components.Add(new EntityComponent(_colors[rand.Next(_colors.Length)], Color.Transparent, 'r'));
            Components.Add(new HealthComponent(Random.Dice(1, 4)));
            var movementComponent = new MovementComponent();
            movementComponent.Speed = 0.8;
            Components.Add(movementComponent);
            Components.Add(new MeleeAttackComponent {
                Dice = 1,
                Sides = 1,
                Speed = 1
            });
            Components.Add(new StatsComponent {
                Strength = 2,
                Dexterity = 11,
                Constitution = 9,
                Intelligence = 2,
                Wisdom = 10,
                Charisma = 4,
                HitDice = 4,
                ArmorClass = 10
            });
            Components.Add(new NameComponent("Rat"));
            Components.Add(new SimpleAIComponent());
        }
    }
}