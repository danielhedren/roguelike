using System.Linq;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Events;
using roguelike.Utils;
using roguelike.World;

namespace roguelike.Handlers
{
    public class AttackHandler : Handler
    {
        public AttackHandler()
        {
            EventBus.Subscribe(typeof(BeforeMeleeAttackEvent), this);
            EventBus.Subscribe(typeof(OnMeleeAttackEvent), this);
        }

        public override void HandleEvent(Event e, Level level)
        {
            if (e.GetType() == typeof(BeforeMeleeAttackEvent)) {
                var ev = (BeforeMeleeAttackEvent) e;

                if (!Utils.Geometry.IsNextTo(ev.Attacker.Get<EntityComponent>().Position, ev.Target.Get<EntityComponent>().Position)) {
                    EventBus.Publish(new OnAttackEvadedEvent {
                        Attacker = ev.Attacker,
                        Target = ev.Target,
                        Damage = ev.Damage
                    });

                    if (ev.InterruptOnCancel) {
                        EventBus.Publish(new InterruptEvent());
                    }

                    return;
                }

                EventBus.Publish(new OnMeleeAttackEvent {
                    Attacker = ev.Attacker,
                    Target = ev.Target,
                    Damage = ev.Damage,
                    InterruptOnCancel = ev.InterruptOnCancel,
                    Interrupt = ev.Attacker == level.GetActors<Player>().First()
                });
            } else {
                var ev = (OnMeleeAttackEvent) e;

                var h = ev.Target.Get<HealthComponent>();
                h.CurrentHealth -= ev.Damage;

                EventBus.Publish(new OnDamageTakenEvent {
                    Attacker = ev.Attacker,
                    Target = ev.Target,
                    Damage = ev.Damage
                });
            }
        }
    }
}