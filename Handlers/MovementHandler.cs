using System.Runtime.InteropServices;
using System.Linq;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Events;
using roguelike.World;

namespace roguelike.Handlers
{
    public class MovementHandler : Handler
    {
        public MovementHandler()
        {
            EventBus.Subscribe(typeof(BeforeMovementEvent), this);
            EventBus.Subscribe(typeof(OnMovementEvent), this);
        }
        
        public override void HandleEvent(Event e, Level level)
        {
            if (e.GetType() == typeof(BeforeMovementEvent))
            {
                var ev = (BeforeMovementEvent) e;

                if (!level.Map.IsWalkable(ev.To.X, ev.To.Y)) {
                    if (ev.InterruptOnCancel) {
                        EventBus.Publish(new InterruptEvent());
                    }
                    
                    return;
                }

                var entities = level.GetActors<Actor>().Where(x => x.Has<EntityComponent>()).Select(x => x.Get<EntityComponent>());

                foreach (var entity in entities)
                {
                    if (entity.Position == ev.To) {
                        if (ev.InterruptOnCancel) {
                            EventBus.Publish(new InterruptEvent());
                        }

                        return;
                    }
                }

                EventBus.Publish(new OnMovementEvent {
                    Actor = ev.Actor,
                    From = ev.From,
                    To = ev.To,
                    Interrupt = ev.Actor == level.GetActors<Player>().First()
                });
            } else if (e.GetType() == typeof(OnMovementEvent))
            {
                var ev = (OnMovementEvent) e;

                var entity = ev.Actor.Get<EntityComponent>();
                entity.Position = ev.To;
            }
        }
    }
}