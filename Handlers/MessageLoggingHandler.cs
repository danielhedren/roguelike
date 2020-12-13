using roguelike.Events;
using roguelike.Utils;
using roguelike.Engine;

namespace roguelike.Handlers
{
    public class MessageLoggingHandler : Handler
    {
        public MessageLoggingHandler(World world) : base(world)
        {
            Subscribe(typeof(OnAttackEvadedEvent));
        }

        public override void HandleEvent(Event e)
        {
            if (e.GetType() == typeof(OnAttackEvadedEvent)) {
                var ev = (OnAttackEvadedEvent) e;

                Logging.Log($"{ev.GetType()}: {ev.IntendedTarget.GetType()} evaded {ev.Attacker.GetType()}s attack!");
            }
        }
    }
}