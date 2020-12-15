using roguelike.Events;
using roguelike.Engine;
using roguelike.Components;

namespace roguelike.Handlers
{
    public class DamageTakenHandler : Handler
    {
        public DamageTakenHandler(World world) : base(world)
        {
            Subscribe(typeof(OnDamageTakenEvent));
        }

        public override void HandleEvent(Event e)
        {
            var ev = (OnDamageTakenEvent) e;

            var health = ev.Target.Get<HealthComponent>();

            if (health.IsDead) {
                _world.EventBus.Publish(new OnDeathEvent {
                    Attacker = ev.Attacker,
                    Target = ev.Target
                });
            }
        }
    }
}