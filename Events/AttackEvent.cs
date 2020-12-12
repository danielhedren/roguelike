using roguelike.Actors;

namespace roguelike.Events
{
    public abstract class AttackEvent : Event
    {
        public Actor Attacker { get; set; }
        public Actor Target { get; set; }
        public double Damage { get; set; }
    }
}