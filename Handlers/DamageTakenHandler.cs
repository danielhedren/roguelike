using roguelike.Events;
using roguelike.Utils;
using roguelike.Engine;
using roguelike.Actors;
using roguelike.Components;

namespace roguelike.Handlers
{
    public class DamageTakenHandler : Handler
    {
        public DamageTakenHandler(World world) : base(world)
        {
            Subscribe(typeof(OnDamageTakenEvent));
        }

        public override void HandleEvent(Event e)
        {
            var ev = (OnDamageTakenEvent) e;

            var health = ev.Target.Get<HealthComponent>();

            Logging.Log($"{ev.GetType().Name}: {ev.Attacker.GetType().Name} damaged {ev.Target.GetType().Name} for {ev.Damage} damage!");

            if (health.IsDead) {
                Logging.Log($"{ev.GetType().Name}: {ev.Target.GetType().Name} died!");

                if (ev.Target.GetType() == typeof(Player)) {
                    Logging.Log($"You died!");
                }

                var experience = ev.Attacker.Get<ExperienceComponent>();
                if (experience != null) {
                    var xpGain = ev.Target.Get<StatsComponent>()?.ExperienceGained ?? 0;
                    experience.Experience += xpGain;
                }

                var corpse = new Corpse();
                corpse.Get<EntityComponent>().Position = ev.Target.Get<EntityComponent>().Position;
                _world.CurrentLevel.Actors.Add(corpse);

                ev.Target.Components.Clear();
                _world.CurrentLevel.Actors.Remove(ev.Target);
            }
        }
    }
}