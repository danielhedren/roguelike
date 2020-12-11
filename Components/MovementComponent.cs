using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.World;

namespace roguelike.Components
{
    public class MovementComponent : Component
    {
        public double Speed { get; set; } = 1;
        public Point Movement { get; set; }
    }
}