using roguelike.Actors;

namespace roguelike.Events
{
    public class OnLevelGainedEvent : Event
    {
        public Actor Target { get; set; }
        public int PreviousLevel { get; set; }
        public int NewLevel { get; set; }
    }
}