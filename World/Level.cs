using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Systems;
using roguelike.Events;

namespace roguelike.World
{
    public class Level
    {
        public RogueSharp.Map Map { get; set; }
        public List<Actor> Actors { get; set; } = new List<Actor>();
        public List<Systems.System> Systems { get; set; } = new List<Systems.System>();

        public Level(int width, int height)
        {
            Map = RogueSharp.Map.Create(new RogueSharp.MapCreation.CaveMapCreationStrategy<RogueSharp.Map>(width, height, 45, 2, 3));
            Systems.Add(new MovementSystem());
            Systems.Add(new SimpleAISystem());

            Systems.Add(new TurnSystem());
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
            foreach (var system in Systems)
            {
                system.Update(this);
            }

            while (EventBus.HandleNext(this))
            {

            }
        }
    }
}