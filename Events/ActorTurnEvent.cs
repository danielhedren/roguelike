using roguelike.Actors;

namespace roguelike.Events
{
    public class ActorTurnEvent : Event
    {
        public Actor Actor { get; set; }
    }
}