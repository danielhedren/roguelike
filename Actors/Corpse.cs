using Microsoft.Xna.Framework;
using roguelike.Components;

namespace roguelike.Actors
{
    public class Corpse : Actor
    {
        public Corpse()
        {
            var entity = new EntityComponent(Color.Gray, Color.Transparent, '%');
            entity.IsWalkable = true;

            Components.Add(entity);
        }
    }
}