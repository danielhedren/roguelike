using System;
using System.IO;
using Microsoft.Xna.Framework;
using roguelike.Consoles;
using roguelike.Engine;
using Console = SadConsole.Console;

namespace roguelike
{
    public static class Program
    {
        public const int Width = 100;
        public const int Height = 45;

        [STAThread]
        static void Main()
        {
            SadConsole.Game.Create(Width, Height);

            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;

            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        static void Init()
        {
            var world = new World();
            world.CreateLevel();
        }
        static void Update(GameTime gameTime)
        {

        }
    }
}
