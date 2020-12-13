using roguelike.Events;
using roguelike.Engine;

namespace roguelike.Handlers
{
    public abstract class Handler
    {
        protected World _world;

        public Handler(World world)
        {
            _world = world;
        }

        public abstract void HandleEvent(Event e);
        public void Subscribe(System.Type type)
        {
            _world.EventBus.Subscribe(type, this);
        }
    }
}