using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using roguelike.Components;

namespace roguelike.Actors.Features
{
    public class Stairs : Feature
    {
        public Stairs()
        {
            var entity = new EntityComponent(Color.Gold, Color.Transparent, '>');
            entity.IsWalkable = true;
            entity.StayRevealed = true;
            Components.Add(entity);
        }
    }
}