using roguelike.Events;
using roguelike.Utils;
using roguelike.Engine;

namespace roguelike.Handlers
{
    public class TurnHandler : Handler
    {
        public TurnHandler(World world) : base(world)
        {
            Subscribe(typeof(ActorTurnEvent));
        }

        public override void HandleEvent(Event e)
        {
            var ev = (ActorTurnEvent) e;

            if (!ev.Handled) {
                _world.EventBus.Publish(new ActorTurnEvent {
                    Actor = ev.Actor,
                    ActivateIn = 1,
                    Interrupt = ev.Interrupt
                });
            }
        }
    }
}