using Microsoft.Xna.Framework;
using roguelike.Components;

namespace roguelike.Actors
{
    public class Chest : Actor
    {
        public Chest()
        {
            Components.Add(new EntityComponent(Color.Gold, Color.Transparent, 240));
        }
    }
}