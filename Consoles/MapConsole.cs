using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Actors.Monsters;
using roguelike.Events;
using roguelike.Engine;
using ImageMagick;
using roguelike.Handlers;

namespace roguelike.Consoles
{
    public class MapConsole : ContainerConsole
    {
        public Console Console { get; }
        public Console UIConsole { get; }
        public Console MessageConsole { get; }
        public World World { get; set; }

        private MagickImageCollection _frames;
        private bool _record = false;

        public MapConsole()
        {
            Console = new Console(Program.Width, Program.Height);
            Console.Parent = this;

            UIConsole = new Console(18, Program.Height - 2);
            UIConsole.Parent = this;
            UIConsole.Position = new Point(Program.Width - 19, 1);

            MessageConsole = new Console(Program.Width, 5);
            MessageConsole.Parent = this;
            MessageConsole.Position = new Point(0, Program.Height - 5);
        }

        public void Draw()
        {
            Console.Clear();

            var player = World.Player;
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

            var player = World.Player;
            var stats = player.Get<StatsComponent>();
            var experience = player.Get<ExperienceComponent>();

            UIConsole.Print(0, 2, $"{"Str: " + stats.Strength, -10}{"Dex: " + stats.Dexterity,-10}");
            UIConsole.Print(0, 3, $"{"Con: " + stats.Constitution, -10}{"Int: " + stats.Intelligence,-10}");

            var health = player.Get<HealthComponent>();

            UIConsole.Print(0, 4, $"HP: {health.CurrentHealth}/{health.MaxHealth}");
            UIConsole.Print(0, 5, $"{"AC: " + (AttackHandler.GetArmorClass(player)), -10}{"ToHit: " + (AttackHandler.GetAttackModifier(player)), -10}");
            UIConsole.Print(0, 7, $"{"XP: " + experience.Experience, -10}{"Nxt: " + experience.ExperienceToNextLevel, -10}");
            UIConsole.Print(0, 8, $"{"Level: " + experience.Level, -10}");

            UIConsole.Print(0, 9, $"Dungeon level: {World.CurrentLevelNumber}");

            var monsters = World.CurrentLevel.GetActors<Monster>().Where(m => {
                var e = m.Get<EntityComponent>();
                return World.CurrentLevel.Map.IsInFov(e.X, e.Y);
            }).ToArray();

            for (int i = 0; i < monsters.Count(); i++) {
                var monster = monsters[i];
                var mHealth = monster.Get<HealthComponent>();
                UIConsole.Print(0, 11 + i, $"{monster.Get<NameComponent>()?.Name} {mHealth.CurrentHealth}/{mHealth.MaxHealth}");
            }

            MessageConsole.Clear();
            var line = 0;
            var count = World.MessageLog.Count();
            foreach (var message in World.MessageLog.TakeLast(5).Reverse()) {
                MessageConsole.Print(0, line++, $"#{count--} {message.Message}", message.Color);
            }
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyPressed(Keys.R)) {
                if (!_record) {
                    _record = true;

                    _frames = new MagickImageCollection();
                } else {
                    _record = false;
                    _frames.First().AnimationIterations = 0;

                    _frames.Optimize();

                    System.IO.Directory.CreateDirectory("recordings");
                    _frames.Write($"recordings/recording_{System.DateTime.Now.ToFileTime()}.gif");
                }
            }
            
            if (World.CurrentLevel != null) {
                var player = World.Player;

                if (player != null) {
                    var handledInput = player.ProcessKeyboard(info, World);

                    if (handledInput) {
                        if (_record)
                        {
                            using (var stream = new System.IO.MemoryStream())
                            {
                                SadConsole.Global.RenderOutput.SaveAsPng(stream, SadConsole.Global.RenderWidth, SadConsole.Global.RenderHeight);
                                stream.Seek(0, 0);
                                _frames.Add(new MagickImage(stream));
                                _frames.Last().AnimationDelay = 10;
                            }
                        }

                        World.Update();
                    }
                }
            }

            Draw();

            return true;
        }
    }
}