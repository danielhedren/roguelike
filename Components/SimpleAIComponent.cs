using Microsoft.Xna.Framework;

namespace roguelike.Components
{
    public class SimpleAIComponent : Component
    {
        public Point? PlayerLastSeen { get; set; } = null;
    }
}