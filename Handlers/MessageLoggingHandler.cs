using roguelike.Events;
using roguelike.Utils;
using roguelike.World;

namespace roguelike.Handlers
{
    public class MessageLoggingHandler : IHandler
    {
        public MessageLoggingHandler()
        {
            EventBus.Subscribe(typeof(OnDamageTakenEvent), this);
            EventBus.Subscribe(typeof(OnAttackEvadedEvent), this);
        }

        public void HandleEvent(Event e, Level level)
        {
            if (e.GetType() == typeof(OnDamageTakenEvent))
            {
                var ev = (OnDamageTakenEvent) e;

                Logging.Log($"{ev.GetType()}: {ev.Attacker.GetType()} damaged {ev.Target.GetType()} for {ev.Damage} damage!");
            } else if (e.GetType() == typeof(OnAttackEvadedEvent)) {
                var ev = (OnAttackEvadedEvent) e;

                Logging.Log($"{ev.GetType()}: {ev.Target.GetType()} evaded {ev.Attacker.GetType()}s attack!");
            }
        }
    }
}