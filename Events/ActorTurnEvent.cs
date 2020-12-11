using roguelike.Actors;

namespace roguelike.Events
{
    public class ActorTurnEvent : Event
    {
        public int Id { get; set; }
        public Actor Actor { get; set; }
    }
}