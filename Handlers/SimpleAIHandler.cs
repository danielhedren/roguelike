using System.Data;
using System.Linq;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Events;
using roguelike.Engine;

namespace roguelike.Handlers
{
    public class SimpleAIHandler : Handler
    {
        public SimpleAIHandler(World world) : base(world)
        {
            Subscribe(typeof(ActorTurnEvent));
        }

        public override void HandleEvent(Event e)
        {
            var ev = (ActorTurnEvent)e;

            if (ev.Actor.Has<SimpleAIComponent>())
            {
                var level = _world.CurrentLevel;

                var player = _world.Player;
                var playerEntity = player.Get<EntityComponent>();
                var entity = ev.Actor.Get<EntityComponent>();
                var movement = ev.Actor.Get<MovementComponent>();
                var ai = ev.Actor.Get<SimpleAIComponent>();
                var attack = ev.Actor.Get<MeleeAttackComponent>();

                if (entity != null && attack != null && Utils.IsNextTo(entity.Position, playerEntity.Position))
                {
                    ai.PlayerLastSeen = playerEntity.Position;

                    _world.EventBus.Publish(new BeforeMeleeAttackEvent
                    {
                        Attacker = ev.Actor,
                        IntendedTarget = player,
                        TargetPoint = playerEntity.Entity.Position,
                        ActivateIn = attack.Speed
                    });

                    return;
                }

                if (entity != null && movement != null)
                {
                    Point to = entity.Entity.Position;

                    var cellsToPlayer = level.Map.GetCellsAlongLine(entity.X, entity.Y, playerEntity.X, playerEntity.Y).Skip(1);
                    if (cellsToPlayer.Count() >= 0 && cellsToPlayer.Count() < 10 && cellsToPlayer.All(x => x.IsTransparent))
                    {
                        ai.PlayerLastSeen = playerEntity.Position;
                    }

                    RogueSharp.ICell pathCell = null;
                    if (ai.PlayerLastSeen != null)
                    {
                        var pathMap = level.Map.Clone();
                        pathMap.SetWalkable(entity.X, entity.Y, true);
                        pathMap.SetWalkable(ai.PlayerLastSeen.Value.X, ai.PlayerLastSeen.Value.Y, true);

                        var pathFinder = new RogueSharp.PathFinder(pathMap);
                        var path = pathFinder.TryFindShortestPath(pathMap.GetCell(entity.X, entity.Y), pathMap.GetCell(ai.PlayerLastSeen.Value.X, ai.PlayerLastSeen.Value.Y));
                        pathCell = path?.TryStepForward();

                        if (pathCell != null)
                        {
                            to = new Point(pathCell.X, pathCell.Y);
                        }
                    }

                    if (pathCell == null)
                    {
                        to = entity.Entity.Position;
                        to.X += Utils.Next(-1, 2);
                        to.Y += Utils.Next(-1, 2);
                    }

                    ev.Handled = true;

                    _world.EventBus.Publish(new BeforeMovementEvent
                    {
                        Actor = ev.Actor,
                        From = entity.Entity.Position,
                        To = to,
                        ActivateIn = movement.Speed
                    });

                    _world.EventBus.Publish(new ActorTurnEvent
                    {
                        Actor = ev.Actor,
                        ActivateIn = movement.Speed
                    });
                }
            }
        }
    }
}