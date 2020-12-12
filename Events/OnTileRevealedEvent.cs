using Microsoft.Xna.Framework;

namespace roguelike.Events
{
    public class OnTileRevealedEvent : Event {
        public Point Tile { get; set; }
    }
}