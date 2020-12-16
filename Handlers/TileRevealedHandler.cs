using System;
using System.Linq;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Events;
using roguelike.Engine;

namespace roguelike.Handlers
{
    public class TileRevealedHandler : Handler
    {
        public TileRevealedHandler(World world) : base(world)
        {
            Subscribe(typeof(OnTileRevealedEvent));
        }

        public override void HandleEvent(Event e)
        {
            var ev = (OnTileRevealedEvent) e;

            var health = _world.Player.Get<HealthComponent>();
            health.CurrentHealth = Math.Min(health.CurrentHealth + 0.5, health.MaxHealth);
        }
    }
}