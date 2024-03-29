using Microsoft.Xna.Framework;
using roguelike.Actors;

namespace roguelike.Events
{
    public abstract class AttackEvent : Event
    {
        public Actor Attacker { get; set; }
        public Actor IntendedTarget { get; set; }
        public Point TargetPoint { get; set; }
        public double Damage { get; set; }
    }
}