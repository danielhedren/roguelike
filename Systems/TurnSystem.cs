using roguelike.Events;
using roguelike.Utils;
using roguelike.World;

namespace roguelike.Systems
{
    public class TurnSystem : System
    {
        public TurnSystem()
        {
            EventBus.Subscribe(typeof(ActorTurnEvent), this);
        }

        public override void HandleEvent(Event e, Level level)
        {
            var ev = (ActorTurnEvent) e;

            if (!ev.Handled) {
                EventBus.Publish(new ActorTurnEvent {
                    Actor = ev.Actor,
                    ActivateIn = 1,
                    Interrupt = ev.Interrupt
                });
            }
        }

        public override void Update(Level level)
        {
        }
    }
}