using roguelike.Events;
using roguelike.Utils;
using roguelike.World;

namespace roguelike.Handlers
{
    public class MessageLoggingHandler : IHandler
    {
        public MessageLoggingHandler()
        {
            EventBus.Subscribe(typeof(OnAttackEvadedEvent), this);
        }

        public void HandleEvent(Event e, Level level)
        {
            if (e.GetType() == typeof(OnAttackEvadedEvent)) {
                var ev = (OnAttackEvadedEvent) e;

                Logging.Log($"{ev.GetType()}: {ev.IntendedTarget.GetType()} evaded {ev.Attacker.GetType()}s attack!");
            }
        }
    }
}