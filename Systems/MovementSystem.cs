using System.Linq;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Events;
using roguelike.World;

namespace roguelike.Systems
{
    public class MovementSystem : System
    {
        public MovementSystem()
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
                });
            } else if (e.GetType() == typeof(OnMovementEvent))
            {
                var ev = (OnMovementEvent) e;

                var entity = ev.Actor.Get<EntityComponent>();
                entity.Position = ev.To;
            }
        }

        public override void Update(Level level)
        {
            return;

            var actors = level.GetActors<Actor>().Where(x => x.Has<MovementComponent>() && x.Has<EntityComponent>());

            foreach (var a in actors)
            {
                var movement = a.Get<MovementComponent>();
                var entity = a.Get<EntityComponent>();

                var previousPosition = entity.Position;
                var newPosition = entity.Position + movement.Movement;

                movement.Movement = Point.Zero;

                if (!level.Map.IsWalkable(newPosition.X, newPosition.Y))
                {
                    newPosition = previousPosition;
                } else {
                    foreach (var a2 in actors)
                    {
                        if (a.Id == a2.Id) continue;

                        if (a2.Get<EntityComponent>()?.Position == newPosition && a2.Get<HealthComponent>()?.CurrentHealth > 0)
                        {
                            newPosition = previousPosition;
                            break;
                        }
                    }
                }

                if (newPosition != previousPosition) {
                    EventBus.Publish(new OnMovementEvent {
                        Actor = a,
                        From = previousPosition,
                        To = newPosition,
                        ActivateIn = movement.Speed
                    });
                }
            }
        }
    }
}