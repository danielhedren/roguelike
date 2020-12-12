using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Events;
using roguelike.Handlers;

namespace roguelike.World
{
    public class Level
    {
        public RogueSharp.Map Map { get; set; }
        public List<Actor> Actors { get; set; } = new List<Actor>();
        public List<IHandler> Handlers { get; set; } = new List<IHandler>();

        public Level(int width, int height)
        {
            Map = RogueSharp.Map.Create(new RogueSharp.MapCreation.CaveMapCreationStrategy<RogueSharp.Map>(width, height, 45, 2, 3));
            Handlers.Add(new MovementHandler());
            Handlers.Add(new SimpleAIHandler());
            Handlers.Add(new AttackHandler());
            Handlers.Add(new MessageLoggingHandler());
            Handlers.Add(new DamageTakenHandler());
            Handlers.Add(new TileRevealedHandler());

            Handlers.Add(new TurnHandler());
        }

        public List<T> GetActors<T>() where T : Actor
        {
            return Actors.Where(x => x.GetType().IsSubclassOf(typeof(T)) || x.GetType().Equals(typeof(T))).Select(x => (T) x).ToList();
        }

        public List<T> GetComponents<T>() where T : Component
        {
            var components = new List<T>();

            foreach (var actor in Actors)
            {
                var component = actor.Get<T>();
                if (component != null) {
                    components.Add(component);
                }
            }

            return components;
        }

        public void Update()
        {
            while (EventBus.HandleNext(this))
            {

            }
        }
    }
}