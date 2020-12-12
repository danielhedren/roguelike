using System.Linq;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Events;
using roguelike.Utils;
using roguelike.World;

namespace roguelike.Handlers
{
    public class AttackHandler : IHandler
    {
        public AttackHandler()
        {
            EventBus.Subscribe(typeof(BeforeMeleeAttackEvent), this);
            EventBus.Subscribe(typeof(OnMeleeAttackEvent), this);
        }

        public void HandleEvent(Event e, Level level)
        {
            if (e.GetType() == typeof(BeforeMeleeAttackEvent)) {
                var ev = (BeforeMeleeAttackEvent) e;

                if (!level.Actors.Contains(ev.Attacker)) {
                    if (ev.InterruptOnCancel) {
                        EventBus.Publish(new InterruptEvent());
                    }

                    return;
                }

                if (ev.IntendedTarget.Get<EntityComponent>()?.Position != ev.TargetPoint) {
                    EventBus.Publish(new OnAttackEvadedEvent {
                        Attacker = ev.Attacker,
                        IntendedTarget = ev.IntendedTarget,
                        Damage = ev.Damage
                    });

                    if (ev.InterruptOnCancel) {
                        EventBus.Publish(new InterruptEvent());
                    }

                    return;
                }

                EventBus.Publish(new OnMeleeAttackEvent {
                    Attacker = ev.Attacker,
                    IntendedTarget = ev.IntendedTarget,
                    Damage = ev.Damage,
                    InterruptOnCancel = ev.InterruptOnCancel,
                    Interrupt = ev.Attacker == level.GetActors<Player>().First()
                });
            } else {
                var ev = (OnMeleeAttackEvent) e;

                var h = ev.IntendedTarget.Get<HealthComponent>();
                h.CurrentHealth -= ev.Damage;

                EventBus.Publish(new OnDamageTakenEvent {
                    Attacker = ev.Attacker,
                    Target = ev.IntendedTarget,
                    Damage = ev.Damage
                });
            }
        }
    }
}