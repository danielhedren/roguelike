using roguelike.Events;
using roguelike.World;

namespace roguelike.Systems
{
    public abstract class System
    {
        public abstract void Update(Level level);
        public abstract void HandleEvent(Event e, Level level);
    }
}