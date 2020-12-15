using roguelike.Events;
using roguelike.Engine;
using Microsoft.Xna.Framework;

namespace roguelike.Handlers
{
    public class MessageLoggingHandler : Handler
    {
        public MessageLoggingHandler(World world) : base(world)
        {
            Subscribe(typeof(OnAttackEvadedEvent));
            Subscribe(typeof(OnAttackRollFailedEvent));
            Subscribe(typeof(OnDamageTakenEvent));
            Subscribe(typeof(OnDeathEvent));
            Subscribe(typeof(OnExperienceGainedEvent));
            Subscribe(typeof(OnLevelGainedEvent));
        }

        public override void HandleEvent(Event e)
        {
            if (e.GetType() == typeof(OnAttackEvadedEvent)) {
                var ev = (OnAttackEvadedEvent) e;

                _world.MessageLog.Add(new MessageLogMessage {
                    Message = $"{ev.GetType().Name}: {ev.IntendedTarget.GetType().Name} moved out of the way of {ev.Attacker.GetType().Name}s attack!",
                    Color = Color.White
                });
            } else if (e.GetType() == typeof(OnAttackRollFailedEvent)) {
                var ev = (OnAttackRollFailedEvent) e;

                _world.MessageLog.Add(new MessageLogMessage {
                    Message = $"{ev.GetType().Name}: {ev.Attacker.GetType().Name} missed {ev.IntendedTarget.GetType().Name}! ({ev.Roll} / {ev.Required})",
                    Color = Color.White
                });
            } else if (e.GetType() == typeof(OnDamageTakenEvent)) {
                var ev = (OnDamageTakenEvent) e;

                _world.MessageLog.Add(new MessageLogMessage {
                    Message = $"{ev.GetType().Name}: {ev.Attacker.GetType().Name} damaged {ev.Target.GetType().Name} for {ev.Damage} damage!",
                    Color = Color.Orange
                });
            } else if (e.GetType() == typeof(OnDeathEvent)) {
                var ev = (OnDeathEvent) e;

                _world.MessageLog.Add(new MessageLogMessage {
                    Message = $"{ev.GetType().Name}: {ev.Attacker.GetType().Name} killed {ev.Target.GetType().Name}!",
                    Color = Color.Red
                });
            } else if (e.GetType() == typeof(OnExperienceGainedEvent)) {
                var ev = (OnExperienceGainedEvent) e;

                _world.MessageLog.Add(new MessageLogMessage {
                    Message = $"{ev.GetType().Name}: {ev.Target.GetType().Name} gained {ev.Experience} experience!",
                    Color = Color.LightGreen
                });
            } else if (e.GetType() == typeof(OnLevelGainedEvent)) {
                var ev = (OnLevelGainedEvent) e;

                _world.MessageLog.Add(new MessageLogMessage {
                    Message = $"{ev.GetType().Name}: {ev.Target.GetType().Name} gained {ev.NewLevel - ev.PreviousLevel} level(s)!",
                    Color = Color.LightGreen
                });
            }
        }
    }
}