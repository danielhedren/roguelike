using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using roguelike.Components;

namespace roguelike.Actors
{
    public class Stairs : Actor
    {
        public Stairs()
        {
            var entity = new EntityComponent(Color.Gold, Color.Transparent, '>');
            entity.IsWalkable = true;
            Components.Add(entity);
        }
    }
}