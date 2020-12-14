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
            Subscribe(typeof(OnAttackRollFailedEvent));
        }

        public override void HandleEvent(Event e)
        {
            if (e.GetType() == typeof(OnAttackEvadedEvent)) {
                var ev = (OnAttackEvadedEvent) e;

                Logging.Log($"{ev.GetType().Name}: {ev.IntendedTarget.GetType().Name} moved out of the way of {ev.Attacker.GetType().Name}s attack!");
            } else if (e.GetType() == typeof(OnAttackRollFailedEvent)) {
                var ev = (OnAttackRollFailedEvent) e;

                Logging.Log($"{ev.GetType().Name}: {ev.Attacker.GetType().Name} missed {ev.IntendedTarget.GetType().Name}! ({ev.Roll} / {ev.Required})");
            }
        }
    }
}