using System.Linq;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Events;
using roguelike.Utils;
using roguelike.World;

namespace roguelike.Handlers
{
    public class SimpleAIHandler : IHandler
    {
        public SimpleAIHandler()
        {
            EventBus.Subscribe(typeof(ActorTurnEvent), this);
        }

        public void HandleEvent(Event e, Level level)
        {
            var ev = (ActorTurnEvent) e;

            if (ev.Actor.Has<SimpleAIComponent>())
            {
                var player = level.GetActors<Player>().First();
                var playerEntity = player.Get<EntityComponent>();
                var entity = ev.Actor.Get<EntityComponent>();
                var movement = ev.Actor.Get<MovementComponent>();
                var ai = ev.Actor.Get<SimpleAIComponent>();
                var attack = ev.Actor.Get<MeleeAttackComponent>();

                if (entity != null && attack != null && level.Map.GetBorderCellsInSquare(entity.X, entity.Y, 1).Any(x => x.X == playerEntity.X && x.Y == playerEntity.Y))
                {
                    EventBus.Publish(new BeforeMeleeAttackEvent {
                        Attacker = ev.Actor,
                        Target = player,
                        Damage = attack.Damage,
                        ActivateIn = attack.Speed
                    });

                    return;
                }

                if (entity != null && movement != null) {
                    var cellsToPlayer = level.Map.GetCellsAlongLine(entity.X, entity.Y, playerEntity.X, playerEntity.Y).Skip(1);
                    Point to;

                    if (cellsToPlayer.Count() > 0 || cellsToPlayer.All(x => x.IsTransparent) || ai.PlayerLastSeen != null) {
                        ai.PlayerLastSeen = playerEntity.Position;

                        var pathFinder = new RogueSharp.PathFinder(level.Map);
                        var path = pathFinder.ShortestPath(level.Map.GetCell(entity.X, entity.Y), level.Map.GetCell(ai.PlayerLastSeen.Value.X, ai.PlayerLastSeen.Value.Y));
                        var cell = path.StepForward();
                        
                        to = new Point(cell.X, cell.Y);

                        if (path.CurrentStep == path.End) {
                            ai.PlayerLastSeen = null;
                        }
                    } else {
                        to = entity.Entity.Position + new Point(Random.Next(-1, 2), Random.Next(-1, 2));
                    }
                    
                    ev.Handled = true;

                    EventBus.Publish(new BeforeMovementEvent {
                        Actor = ev.Actor,
                        From = entity.Entity.Position,
                        To = to,
                        ActivateIn = movement.Speed
                    });

                    EventBus.Publish(new ActorTurnEvent {
                        Actor = ev.Actor,
                        ActivateIn = movement.Speed
                    });
                }
            }
        }
    }
}