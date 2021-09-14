using System.Linq;
using System;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Consoles;
using roguelike.Events;
using roguelike.Handlers;

namespace roguelike.Engine
{
    public struct MessageLogMessage
    {
        public string Message { get; set; }
        public Color Color { get; set; }
    }

    public class World
    {
        public Level CurrentLevel { get; set; }
        public int CurrentLevelNumber { get; set; } = 0;
        public List<MessageLogMessage> MessageLog { get; set; } = new List<MessageLogMessage>();
        public List<Handler> Handlers { get; set; } = new List<Handler>();
        public EventBus EventBus { get; set; }
        public Player Player { get; set; } = new Player();

        public MapConsole MapConsole { get; set; }
        public InventoryConsole InventoryConsole { get; set; }
        public InspectionConsole EventConsole { get; set; }

        public World()
        {
            EventBus = new EventBus(this);

            RegisterHandlers();

            MapConsole = new MapConsole();
            MapConsole.World = this;

            InventoryConsole = new InventoryConsole();
            InventoryConsole.World = this;

            EventConsole = new InspectionConsole();
            EventConsole.World = this;

            SadConsole.Global.CurrentScreen = MapConsole;
            SadConsole.Global.CurrentScreen.IsFocused = true;
        }

        private void RegisterHandlers()
        {
            var genericRegisterHandler = typeof(EventBus).GetMethod("RegisterHandler");
            var handlerTypes = Assembly.GetAssembly(typeof(Handler)).GetTypes().Where(x => !x.IsAbstract && x.IsSubclassOf(typeof(Handler)));
            foreach (var type in handlerTypes)
            {
                var registerHandler = genericRegisterHandler.MakeGenericMethod(type);
                registerHandler.Invoke(EventBus, null);
            }
        }

        public void CreateLevel()
        {
            CurrentLevelNumber++;
            CurrentLevel = new CaveLevel(this, 80, 40);

            CurrentLevel.Actors.Add(Player);
            CurrentLevel.Initialize();
            MapConsole.Console.Children.Add(Player.Get<Components.EntityComponent>().Entity);

            foreach (var actor in CurrentLevel.Actors)
            {
                EventBus.Publish(new ActorTurnEvent
                {
                    Actor = actor
                });
            }
        }

        public void DestroyLevel()
        {
            MapConsole.Console.Children.Clear();
            CurrentLevel.Actors.Clear();
            EventBus.Events.Clear();
        }

        public void Update()
        {
            while (EventBus.HandleNext())
            {

            }
        }
    }
}