using System.Linq;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Events;
using roguelike.Utils;
using roguelike.World;

namespace roguelike.Systems
{
    public class SimpleAISystem : System
    {
        public SimpleAISystem()
        {
            EventBus.Subscribe(typeof(ActorTurnEvent), this);
        }

        public override void HandleEvent(Event e, Level level)
        {
            var ev = (ActorTurnEvent) e;

            if (ev.Actor.Has<SimpleAIComponent>())
            {
                var player = level.GetActors<Player>().First();
                var playerEntity = player.Get<EntityComponent>();
                var entity = ev.Actor.Get<EntityComponent>();
                var movement = ev.Actor.Get<MovementComponent>();
                var ai = ev.Actor.Get<SimpleAIComponent>();

                if (entity != null && movement != null) {
                    var cellsToPlayer = level.Map.GetCellsAlongLine(entity.X, entity.Y, playerEntity.X, playerEntity.Y).Skip(1);
                    Point to;

                    if (cellsToPlayer.All(x => x.IsTransparent)) {
                        ai.PlayerLastSeen = playerEntity.Position;
                        to = new Point(cellsToPlayer.First().X, cellsToPlayer.First().Y);
                    } else if (ai.PlayerLastSeen != null) {
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

                    EventBus.Publish(new BeforeMovementEvent {
                        Actor = ev.Actor,
                        From = entity.Entity.Position,
                        To = to
                    });
                }
            }
        }

        public override void Update(Level level)
        {
            
        }
    }
}