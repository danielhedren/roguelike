using System.Linq;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Actors.Monsters;
using roguelike.Components;

namespace roguelike.Engine
{
    public class CaveLevel : Level
    {
        public CaveLevel(World world, int width, int height) : base(world, width, height)
        {
            
        }

        public override void Initialize()
        {
            for (int i = 0; i < Utils.Roll(3, 6); i++) {
                Actors.Add(new Rat());
            }

            for (int i = 0; i < Utils.Roll(1, 4); i++) {
                Actors.Add(new GiantFireBeetle());
            }

            for (int i = 0; i < Utils.Roll(1, 6); i++) {
                Actors.Add(new Kobold());
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