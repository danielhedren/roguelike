using roguelike.Actors;
using roguelike.Engine;

namespace roguelike.Events
{
    public class OnExperienceGainedEvent : Event
    {
        public Actor Target { get; set; }
        public int Experience { get; set; }
    }
}