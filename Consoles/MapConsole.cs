using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Actors.Monsters;
using roguelike.Events;
using roguelike.Engine;

namespace roguelike.Consoles
{
    class MapConsole : ContainerConsole
    {
        public Console Console { get; }
        public Console UIConsole { get; }
        public World World { get; set; }

        public MapConsole()
        {
            Console = new Console(Program.Width, Program.Height);
            Console.Parent = this;

            UIConsole = new Console(18, Program.Height - 2);
            UIConsole.Parent = this;
            var color = Color.Gray;
            color.A = 127;
            UIConsole.Fill(null, color, null);
            UIConsole.Position = new Point(Program.Width - 19, 1);
        }

        public void Draw()
        {
            Console.Clear();

            var player = World.CurrentLevel.GetActors<Player>().FirstOrDefault();
            if (player != null) {
                var entity = player.Get<EntityComponent>();

                World.CurrentLevel.Map.ComputeFov(entity.Position.X, entity.Position.Y, 20, true);
            }

            Console.Children.Clear();
            foreach (var e in World.CurrentLevel.GetComponents<EntityComponent>())
            {
                if (World.CurrentLevel.Map.IsInFov(e.X, e.Y)) {
                    Console.Children.Add(e.Entity);
                }
            }

            foreach (var cell in World.CurrentLevel.Map.GetAllCells())
            {
                if (cell.IsInFov && !cell.IsExplored)
                {
                    World.CurrentLevel.Map.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                    
                    World.EventBus.Publish(new OnTileRevealedEvent {
                        Tile = new Point(cell.X, cell.Y)
                    });
                }

                if (cell.IsInFov)
                {
                    if (cell.IsWalkable) {
                        Console.Fill(cell.X, cell.Y, 1, Color.Gray, null, '.');
                    } else {
                        Console.Fill(cell.X, cell.Y, 1, Color.White, Color.Gray, 219);
                    }
                } else if (cell.IsExplored)
                {
                    if (cell.IsWalkable) {
                        Console.Fill(cell.X, cell.Y, 1, Color.DarkGray, Color.Black, '.');
                    } else {
                        Console.Fill(cell.X, cell.Y, 1, Color.DarkGray, Color.Black, 219);
                    }
                }
            }

            DrawUI();
        }

        public void DrawUI()
        {
            UIConsole.Clear();
            UIConsole.Print(0, 0, "Playername");
            UIConsole.Print(0, 2, $"{"Str: 0", -10}{"Agi: 0",-10}");

            var player = World.CurrentLevel.GetActors<Player>().First();
            var health = player.Get<HealthComponent>();

            UIConsole.Print(0, 4, $"Health: {health.CurrentHealth}/{health.MaxHealth}");
            UIConsole.Print(0, 5, $"Dungeon level: {World.CurrentLevelNumber}");

            var monsters = World.CurrentLevel.GetActors<Monster>().Where(m => {
                var e = m.Get<EntityComponent>();
                return World.CurrentLevel.Map.IsInFov(e.X, e.Y);
            }).ToArray();

            for (int i = 0; i < monsters.Count(); i++) {
                var monster = monsters[i];
                var mHealth = monster.Get<HealthComponent>();
                UIConsole.Print(0, 11 + i, $"{monster.Get<NameComponent>()?.Name} {mHealth.CurrentHealth}/{mHealth.MaxHealth}");
            }
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (World.CurrentLevel != null) {
                var player = World.CurrentLevel.GetActors<Player>().FirstOrDefault();

                if (player != null) {
                    var handledInput = player.ProcessKeyboard(info, World);

                    if (handledInput) World.Update();
                }
            }

            Draw();

            return true;
        }
    }
}