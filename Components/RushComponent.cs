using System.Linq;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.World;

namespace roguelike.Components
{
    public class RushComponent : Component
    {
        public override void Update(Actor parent, Level level)
        {
            var movement = parent.Get<MovementComponent>();
            if (movement == null) return;

            var player = level.GetActors<Player>().FirstOrDefault();
            if (player == null) return;

            var entity = parent.Get<EntityComponent>();
            if (entity == null) return;

            var pEntity = player.Get<EntityComponent>();
            if (pEntity == null) return;

            var cells = level.Map.GetCellsAlongLine(entity.X, entity.Y, pEntity.X, pEntity.Y).Skip(1);
            if (cells.Count() == 0) return;

            var visible = cells.All(x => x.IsTransparent);
            if (!visible) return;

            movement.Movement = new Point(cells.First().X - entity.X, cells.First().Y - entity.Y);
        }
    }
}