using System.Linq;
using Microsoft.Xna.Framework;
using roguelike.Actors.Features;
using roguelike.Actors.Items;
using roguelike.Actors.Monsters;
using roguelike.Components;

namespace roguelike.Engine
{
    public class CaveLevel : Level
    {
        public CaveLevel(World world, int width, int height) : base(world, width, height)
        {
            Map = RogueSharp.Map.Create(new RogueSharp.MapCreation.CaveMapCreationStrategy<RogueSharp.Map>(width, height, 45, 2, 3));

            BaseMap = (RogueSharp.Map)Map.Clone();
        }

        public override void Initialize()
        {
            for (int i = 0; i < Utils.Roll(3, 6); i++)
            {
                Actors.Add(new Rat());
            }

            for (int i = 0; i < Utils.Roll(1, 4); i++)
            {
                Actors.Add(new GiantFireBeetle());
            }

            for (int i = 0; i < Utils.Roll(1, 6); i++)
            {
                Actors.Add(new Kobold());
            }

            Actors.Add(new Stairs());

            var rand = new System.Random();
            foreach (var entity in GetComponents<EntityComponent>())
            {
                foreach (var cell in Map.GetAllCells().OrderBy(x => rand.Next()))
                {
                    if (cell.IsWalkable)
                    {
                        entity.Position = new Point(cell.X, cell.Y);
                    }
                }
            }

            var playerPos = _world.Player.Get<EntityComponent>().Position;

            var dagger = new Dagger();
            dagger.Get<EntityComponent>().Position = playerPos;
            Actors.Add(dagger);

            var sword = new Longsword();
            sword.Get<EntityComponent>().Position = playerPos;
            Actors.Add(sword);
        }
    }
}