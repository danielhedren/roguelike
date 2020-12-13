using System.Linq;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Events;
using roguelike.Utils;
using roguelike.Engine;

namespace roguelike.Handlers
{
    public class AttackHandler : Handler
    {
        public AttackHandler(World world) : base(world)
        {
            Subscribe(typeof(BeforeMeleeAttackEvent));
            Subscribe(typeof(OnMeleeAttackEvent));
        }

        public override void HandleEvent(Event e)
        {
            if (e.GetType() == typeof(BeforeMeleeAttackEvent)) {
                var ev = (BeforeMeleeAttackEvent) e;

                if (!_world.CurrentLevel.Actors.Contains(ev.Attacker)) {
                    _world.EventBus.Cancel(e);

                    return;
                }

                if (ev.IntendedTarget.Get<EntityComponent>()?.Position != ev.TargetPoint) {
                    _world.EventBus.Publish(new OnAttackEvadedEvent {
                        Attacker = ev.Attacker,
                        IntendedTarget = ev.IntendedTarget,
                        Damage = ev.Damage
                    });

                    if (ev.InterruptOnCancel) {
                        _world.EventBus.Publish(new InterruptEvent());
                    }

                    return;
                }

                _world.EventBus.Publish(new OnMeleeAttackEvent {
                    Attacker = ev.Attacker,
                    IntendedTarget = ev.IntendedTarget,
                    Damage = ev.Damage,
                    InterruptOnCancel = ev.InterruptOnCancel,
                    Interrupt = ev.Attacker == _world.CurrentLevel.GetActors<Player>().First()
                });
            } else {
                var ev = (OnMeleeAttackEvent) e;

                if (!_world.CurrentLevel.Actors.Contains(ev.Attacker)) {
                    _world.EventBus.Cancel(e);
                    
                    return;
                }

                var h = ev.IntendedTarget.Get<HealthComponent>();
                h.CurrentHealth -= ev.Damage;

                _world.EventBus.Publish(new OnDamageTakenEvent {
                    Attacker = ev.Attacker,
                    Target = ev.IntendedTarget,
                    Damage = ev.Damage
                });
            }
        }
    }
}