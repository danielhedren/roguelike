using roguelike.Events;
using roguelike.Utils;
using roguelike.World;

namespace roguelike.Handlers
{
    public class TurnHandler : Handler
    {
        public TurnHandler()
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
    }
}