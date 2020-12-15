using System.Linq;
using roguelike.Actors;
using roguelike.Components;
using roguelike.Events;
using roguelike.Utils;
using roguelike.Engine;
using roguelike.Actors.Monsters;

namespace roguelike.Handlers
{
    public class AttackHandler : Handler
    {
        public AttackHandler(World world) : base(world)
        {
            Subscribe(typeof(BeforeMeleeAttackEvent));
            Subscribe(typeof(OnMeleeAttackEvent));
        }

        public static int GetArmorClass(Actor actor)
        {
            var stats = actor.Get<StatsComponent>();

            var armorClass = stats?.ArmorClass ?? 0;

            if (actor.GetType().IsSubclassOf(typeof(Player)) || actor.GetType() == typeof(Player)) {
                armorClass += stats?.DexterityModifier ?? 0;
            }

            return armorClass;
        }

        public static int GetProficiencyBonus(Actor actor)
        {
            var level = actor.Get<ExperienceComponent>()?.Level ?? 0;

            return 1 + (int) System.Math.Ceiling((double)level / 4);
        }

        public static int GetAttackModifier(Actor actor)
        {
            var attackModifier = 0;

            if (actor.GetType().IsSubclassOf(typeof(Monster))) {
                attackModifier = actor.Get<MeleeAttackComponent>()?.ToHit ?? 0;
            } else {
                attackModifier = actor.Get<StatsComponent>()?.StrengthModifier ?? 0;
                attackModifier += GetProficiencyBonus(actor);
            }

            return attackModifier;
        }

        public static int GetAttackDamage(Actor actor)
        {
            var attack = actor.Get<MeleeAttackComponent>();

            if (attack == null) return 0;

            return attack.Damage;
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
                    });

                    if (ev.InterruptOnCancel) {
                        _world.EventBus.Publish(new InterruptEvent());
                    }

                    return;
                }

                var targetAC = GetArmorClass(ev.IntendedTarget);
                var attackModifier = GetAttackModifier(ev.Attacker);

                var diceRoll = Random.Dice(1, 20, attackModifier);
                if (diceRoll == 1 || (diceRoll != 20 && diceRoll < targetAC)) {
                    _world.EventBus.Publish(new OnAttackRollFailedEvent {
                        Attacker = ev.Attacker,
                        IntendedTarget = ev.IntendedTarget,
                        Roll = diceRoll,
                        Required = targetAC
                    });

                    if (ev.InterruptOnCancel) {
                        _world.EventBus.Publish(new InterruptEvent());
                    }

                    return;
                }

                var damage = GetAttackDamage(ev.Attacker);

                _world.EventBus.Publish(new OnMeleeAttackEvent {
                    Attacker = ev.Attacker,
                    IntendedTarget = ev.IntendedTarget,
                    Damage = damage,
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