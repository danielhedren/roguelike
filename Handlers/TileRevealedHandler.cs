using System;
using System.Linq;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Events;
using roguelike.World;

namespace roguelike.Handlers
{
    public class TileRevealedHandler : IHandler
    {
        public TileRevealedHandler()
        {
            EventBus.Subscribe(typeof(OnTileRevealedEvent), this);
        }

        public void HandleEvent(Event e, Level level)
        {
            var ev = (OnTileRevealedEvent) e;

            var health = level.GetActors<Player>().First().Get<HealthComponent>();
            health.CurrentHealth = Math.Min(health.CurrentHealth + 0.5, health.MaxHealth);
        }
    }
}