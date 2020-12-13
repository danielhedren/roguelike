using Microsoft.Xna.Framework;

namespace roguelike.Components
{
    public class MovementComponent : Component
    {
        public double Speed { get; set; } = 1;
        public Point Movement { get; set; }
    }
}