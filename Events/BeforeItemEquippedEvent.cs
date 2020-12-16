using roguelike.Actors;
using roguelike.Actors.Items;

namespace roguelike.Events
{
    public class BeforeItemEquippedEvent : Event
    {
        public Item Item { get; set; }
        public Actor Target { get; set; }
    }
}