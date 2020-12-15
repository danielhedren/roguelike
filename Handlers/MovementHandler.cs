using System.Runtime.InteropServices;
using System.Linq;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Events;
using roguelike.Engine;

namespace roguelike.Handlers
{
    public class MovementHandler : Handler
    {
        public MovementHandler(World world) : base(world)
        {
            Subscribe(typeof(BeforeMovementEvent));
            Subscribe(typeof(OnMovementEvent));
        }
        
        public override void HandleEvent(Event e)
        {
            if (e.GetType() == typeof(BeforeMovementEvent))
            {
                var ev = (BeforeMovementEvent) e;

                if (!_world.CurrentLevel.Map.IsWalkable(ev.To.X, ev.To.Y)) {
                    _world.EventBus.Cancel(e);
                    
                    return;
                }

                if (ev.From != ev.Actor.Get<EntityComponent>()?.Position) {
                    _world.EventBus.Cancel(e);

                    return;
                }

                var actors = _world.CurrentLevel.GetActors<Actor>().Where(x => x.Has<EntityComponent>());

                foreach (var actor in actors)
                {
                    if (actor == ev.Actor) continue;

                    var entity = actor.Get<EntityComponent>();
                    if (entity.IsWalkable) continue;

                    var health = actor.Get<HealthComponent>();

                    if (entity.Position == ev.To && (health == null || !health.IsDead)) {
                        _world.EventBus.Cancel(e);

                        return;
                    }
                }

                _world.EventBus.Publish(new OnMovementEvent {
                    Actor = ev.Actor,
                    From = ev.From,
                    To = ev.To,
                    Interrupt = ev.Actor == _world.CurrentLevel.GetActors<Player>().First()
                });
            } else if (e.GetType() == typeof(OnMovementEvent))
            {
                var ev = (OnMovementEvent) e;

                if (!_world.CurrentLevel.Actors.Contains(ev.Actor)) {
                    _world.EventBus.Cancel(e);

                    return;
                }

                var entity = ev.Actor.Get<EntityComponent>();               
                entity.Position = ev.To;
            }
        }
    }
}