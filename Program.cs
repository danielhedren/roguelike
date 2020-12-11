using System;
using Microsoft.Xna.Framework;
using roguelike.Consoles;
using Console = SadConsole.Console;

namespace roguelike
{
    public static class Program
    {
        public const int Width = 100;
        public const int Height = 40;

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
            SadConsole.Global.CurrentScreen = new MapConsole();
            SadConsole.Global.CurrentScreen.IsFocused = true;
        }

        static void Update(GameTime gameTime)
        {

        }
    }
}
