using roguelike.Events;
using roguelike.World;

namespace roguelike.Handlers
{
    public interface IHandler
    {
        void HandleEvent(Event e, Level level);
    }
}