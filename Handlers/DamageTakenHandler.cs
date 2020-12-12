using roguelike.Events;
using roguelike.Utils;
using roguelike.World;
using roguelike.Actors;
using roguelike.Components;

namespace roguelike.Handlers
{
    public class DamageTakenHandler : IHandler
    {
        public DamageTakenHandler()
        {
            EventBus.Subscribe(typeof(OnDamageTakenEvent), this);
        }

        public void HandleEvent(Event e, Level level)
        {
            var ev = (OnDamageTakenEvent) e;

            var health = ev.Target.Get<HealthComponent>();

            Logging.Log($"{ev.GetType()}: {ev.Attacker.GetType()} damaged {ev.Target.GetType()} for {ev.Damage} damage!");

            if (health.IsDead) {
                Logging.Log($"{ev.GetType()}: {ev.Target.GetType()} died!");

                if (ev.Target.GetType() == typeof(Player)) {
                    Logging.Log($"You died!");
                }

                ev.Target.Components.Clear();
                level.Actors.Remove(ev.Target);
            }
        }
    }
}