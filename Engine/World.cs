using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Actors.Monsters;
using roguelike.Components;
using roguelike.Events;
using roguelike.Handlers;

namespace roguelike.Engine
{
    public class World
    {
        public Level CurrentLevel { get; set; }
        public int CurrentLevelNumber { get; set; } = 0;
        public SadConsole.Console Console { get; set; }
        public List<Handler> Handlers { get; set; } = new List<Handler>();
        public EventBus EventBus { get; set; }
        private Player _player { get; set; } = new Player();

        public World()
        {
            EventBus = new EventBus(this);

            EventBus.RegisterHandler<MovementHandler>();
            EventBus.RegisterHandler<SimpleAIHandler>();
            EventBus.RegisterHandler<AttackHandler>();
            EventBus.RegisterHandler<MessageLoggingHandler>();
            EventBus.RegisterHandler<DamageTakenHandler>();
            EventBus.RegisterHandler<TileRevealedHandler>();
            EventBus.RegisterHandler<TurnHandler>();
        }

        public void CreateLevel()
        {
            CurrentLevelNumber++;
            CurrentLevel = new CaveLevel(this, 80, 40);

            CurrentLevel.Actors.Add(_player);
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