using System;
using roguelike.Actors;
using roguelike.World;

namespace roguelike.Components
{
    public abstract class Component
    {
        private static int _currentId = 0;
        public readonly int Id = _currentId++;
        public virtual void Update(Actor parent, Level level) { }
    }
}