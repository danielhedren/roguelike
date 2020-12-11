using Microsoft.Xna.Framework;
using roguelike.Components;

namespace roguelike.Actors.Monsters
{
    public class Rat : Monster
    {
        private static Color[] _colors = new Color[] { Color.Brown, Color.White, Color.Goldenrod, Color.LightGray };
        public Rat()
        {
            var rand = new System.Random();

            Components.Add(new EntityComponent(_colors[rand.Next(_colors.Length)], Color.Transparent, 'r'));
            Components.Add(new HealthComponent(10));
            var movementComponent = new MovementComponent();
            movementComponent.Speed = 0.5;
            Components.Add(movementComponent);
            Components.Add(new MeleeAttackComponent(5));
            Components.Add(new NameComponent("Rat"));
            Components.Add(new SimpleAIComponent());
        }
    }
}