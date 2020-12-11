using Microsoft.Xna.Framework;
using roguelike.Actors;

namespace roguelike.Events
{
    public abstract class MovementEvent : Event
    {
        public Actor Actor { get; set; }
        public Point From { get; set; }
        public Point To { get; set; }
    }
}