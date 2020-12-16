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
        public SadConsole.Console Console { get; set; }
        public List<MessageLogMessage> MessageLog { get; set; } = new List<MessageLogMessage>();
        public List<Handler> Handlers { get; set; } = new List<Handler>();
        public EventBus EventBus { get; set; }
        public Player Player { get; set; } = new Player();

        public MapConsole MapConsole { get; set; }
        public InventoryConsole InventoryConsole { get; set; }

        public World()
        {
            EventBus = new EventBus(this);

            EventBus.RegisterHandler<MovementHandler>();
            EventBus.RegisterHandler<SimpleAIHandler>();
            EventBus.RegisterHandler<AttackHandler>();
            EventBus.RegisterHandler<MessageLoggingHandler>();
            EventBus.RegisterHandler<DamageTakenHandler>();
            EventBus.RegisterHandler<TileRevealedHandler>();
            EventBus.RegisterHandler<DeathHandler>();
            EventBus.RegisterHandler<ExperienceHandler>();
            EventBus.RegisterHandler<ItemHandler>();
            EventBus.RegisterHandler<TurnHandler>();

            MapConsole = new MapConsole();
            MapConsole.World = this;

            InventoryConsole = new InventoryConsole();
            InventoryConsole.World = this;

            SadConsole.Global.CurrentScreen = MapConsole;
            SadConsole.Global.CurrentScreen.IsFocused = true;
        }

        public void CreateLevel()
        {
            CurrentLevelNumber++;
            CurrentLevel = new CaveLevel(this, 80, 40);

            CurrentLevel.Actors.Add(Player);
            CurrentLevel.Initialize();

            foreach (var actor in CurrentLevel.Actors)
            {
                EventBus.Publish(new ActorTurnEvent{
                    Actor = actor
                });
            }
        }

        public void DestroyLevel()
        {
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