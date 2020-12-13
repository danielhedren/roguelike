using System.Linq;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Actors.Monsters;
using roguelike.Components;
using roguelike.Events;

namespace roguelike.Engine
{
    public class CaveLevel : Level
    {
        public CaveLevel(World world, int width, int height) : base(world, width, height)
        {
            
        }

        public override void Initialize()
        {
            for (int i = 0; i < 10; i++) {
                Actors.Add(new Rat());
            }

            Actors.Add(new Stairs());

            var rand = new System.Random();
            foreach (var entity in GetComponents<EntityComponent>())
            {
                foreach (var cell in Map.GetAllCells().OrderBy(x => rand.Next())) {
                    if (cell.IsWalkable) {
                        entity.Position = new Point(cell.X, cell.Y);
                    }
                }
            }
        }
    }
}