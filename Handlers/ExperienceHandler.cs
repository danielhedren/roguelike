using roguelike.Components;
using roguelike.Engine;
using roguelike.Events;
using roguelike.Utils;

namespace roguelike.Handlers
{
    public class ExperienceHandler : Handler
    {
        public ExperienceHandler(World world) : base(world)
        {
            Subscribe(typeof(OnExperienceGainedEvent));
            Subscribe(typeof(OnLevelGainedEvent));
        }

        public override void HandleEvent(Event e)
        {
            if (e.GetType() == typeof(OnExperienceGainedEvent))
            {
                var ev = (OnExperienceGainedEvent) e;

                var experience = ev.Target.Get<ExperienceComponent>();
                if (experience == null) return;

                var level = experience.Level;
                experience.Experience += ev.Experience;

                if (experience.Level != level) {
                    _world.EventBus.Publish(new OnLevelGainedEvent {
                        Target = ev.Target,
                        PreviousLevel = level,
                        NewLevel = experience.Level
                    });
                }
            } else {
                var ev = (OnLevelGainedEvent) e;

                var health = ev.Target.Get<HealthComponent>();
                var stats = ev.Target.Get<StatsComponent>();

                if (health != null && stats != null) {
                    health.MaxHealth += Random.Dice(1, stats.HitDice, stats.ConstitutionModifier);
                    health.CurrentHealth = health.MaxHealth;
                }
            }
        }
    }
}