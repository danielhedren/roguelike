using System.Threading;
using System.Linq;
using System.Net.Security;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Entities;
using SadConsole.Input;
using roguelike.World;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Actors.Monsters;
using roguelike.Events;

namespace roguelike.Consoles
{
    class MapConsole: ContainerConsole
    {
        public Console Console { get; }
        public Console UIConsole { get; }
        public Player Player { get; set; }
        public Level Level { get; set; }

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

            Level = new Level(80, 40);

            Player = new Player();
            Level.Actors.Add(Player);

            for (int i = 0; i < 10; i++) {
                Level.Actors.Add(new Rat());
            }

            foreach (var actor in Level.Actors)
            {
                EventBus.Publish(new ActorTurnEvent{
                    Actor = actor
                });
            }

            var rand = new System.Random();
            foreach (var entity in Level.GetComponents<EntityComponent>())
            {
                foreach (var cell in Level.Map.GetAllCells().OrderBy(x => rand.Next())) {
                    if (cell.IsWalkable) {
                        entity.Position = new Point(cell.X, cell.Y);
                    }
                }
            }
        }

        public void Draw()
        {
            Console.Clear();

            var entity = Player.Get<EntityComponent>();

            Level.Map.ComputeFov(entity.Position.X, entity.Position.Y, 20, true);

            foreach (var e in Level.GetComponents<EntityComponent>())
            {
                if (Level.Map.IsInFov(e.X, e.Y)) {
                    Console.Children.Add(e.Entity);
                } else {
                    Console.Children.Remove(e.Entity);
                }
            }

            foreach (var cell in Level.Map.GetAllCells())
            {
                if (cell.IsInFov)
                {
                    Level.Map.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);

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

            var health = Player.Get<HealthComponent>();

            UIConsole.Print(0, 4, $"Health: {health.CurrentHealth}/{health.MaxHealth}");

            var monsters = Level.GetActors<Monster>().Where(m => {
                var e = m.Get<EntityComponent>();
                return Level.Map.IsInFov(e.X, e.Y);
            }).ToArray();

            for (int i = 0; i < monsters.Count(); i++) {
                var monster = monsters[i];
                var mHealth = monster.Get<HealthComponent>();
                UIConsole.Print(0, 11 + i, $"{monster.Get<NameComponent>()?.Name} {mHealth.CurrentHealth}/{mHealth.MaxHealth}");
            }
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyPressed(Keys.Space)) {
                var actors = Level.Actors;
                Level = new Level(80, 25);
                Level.Actors = actors;
            }

            var handledInput = Player.ProcessKeyboard(info, Level);

            if (handledInput) Level.Update();

            Draw();

            return true;
        }
    }
}