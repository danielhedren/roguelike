using roguelike.Actors;

namespace roguelike.Events
{
    public class OnDeathEvent : Event
    {
        public Actor Attacker { get; set; }
        public Actor Target { get; set; }
    }
}