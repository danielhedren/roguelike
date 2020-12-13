using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using roguelike.Components;
using roguelike.Engine;
using SadConsole.Entities;

namespace roguelike.Actors
{
    public abstract class Actor
    {
        private static int _currentId = 0;
        public readonly int Id = _currentId++;
        public List<Component> Components { get; set; } = new List<Component>();

        public T Get<T>() where T : Component
        {
            return (T) Components.Find(x => x.GetType().Equals(typeof(T)));
        }

        public bool Has<T>() where T : Component
        {
            return Components.FindIndex(x => x.GetType().Equals(typeof(T))) != -1;
        }
    }
}