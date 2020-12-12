using roguelike.Events;
using roguelike.World;

namespace roguelike.Handlers
{
    public abstract class Handler
    {
        public abstract void HandleEvent(Event e, Level level);
    }
}