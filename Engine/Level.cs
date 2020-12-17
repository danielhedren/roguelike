using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Events;
using roguelike.Handlers;

namespace roguelike.Engine
{
    public abstract class Level
    {
        public RogueSharp.Map Map { get; set; }
        public RogueSharp.Map BaseMap { get; set; }
        public List<Actor> Actors { get; set; } = new List<Actor>();
        protected World _world;

        public Level(World world, int width, int height)
        {
            _world = world;
        }

        public List<T> GetActors<T>() where T : Actor
        {
            return Actors.Where(x => x.GetType().IsSubclassOf(typeof(T)) || x.GetType().Equals(typeof(T))).Select(x => (T)x).ToList();
        }

        public List<T> GetComponents<T>() where T : Component
        {
            var components = new List<T>();

            foreach (var actor in Actors)
            {
                var component = actor.Get<T>();
                if (component != null)
                {
                    components.Add(component);
                }
            }

            return components;
        }

        public abstract void Initialize();
    }
}