using System.Runtime.InteropServices;
using System.Linq;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Events;
using roguelike.World;

namespace roguelike.Handlers
{
    public class MovementHandler : IHandler
    {
        public MovementHandler()
        {
            EventBus.Subscribe(typeof(BeforeMovementEvent), this);
            EventBus.Subscribe(typeof(OnMovementEvent), this);
        }
        
        public void HandleEvent(Event e, Level level)
        {
            if (e.GetType() == typeof(BeforeMovementEvent))
            {
                var ev = (BeforeMovementEvent) e;

                if (!level.Map.IsWalkable(ev.To.X, ev.To.Y)) {
                    ev.Cancel();
                    
                    return;
                }

                var actors = level.GetActors<Actor>().Where(x => x.Has<EntityComponent>());

                foreach (var actor in actors)
                {
                    if (actor == ev.Actor) continue;

                    var entity = actor.Get<EntityComponent>();
                    if (entity.IsWalkable) continue;

                    var health = actor.Get<HealthComponent>();

                    if (entity.Position == ev.To && (health == null || !health.IsDead)) {
                        ev.Cancel();

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

                if (!level.Actors.Contains(ev.Actor)) {
                    ev.Cancel();

                    return;
                }

                var entity = ev.Actor.Get<EntityComponent>();
                entity.Position = ev.To;
            }
        }
    }
}