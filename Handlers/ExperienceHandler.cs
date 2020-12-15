using roguelike.Components;
using roguelike.Engine;
using roguelike.Events;

namespace roguelike.Handlers
{
    public class ExperienceHandler : Handler
    {
        public double ExperienceModifier { get; set; } = 10; // Useful for testing purposes
        public ExperienceHandler(World world) : base(world)
        {
            Subscribe(typeof(BeforeExperienceGainedEvent));
            Subscribe(typeof(OnExperienceGainedEvent));
            Subscribe(typeof(OnLevelGainedEvent));
        }

        public override void HandleEvent(Event e)
        {
            if (e.GetType() == typeof(BeforeExperienceGainedEvent))
            {
                var ev = (BeforeExperienceGainedEvent) e;

                _world.EventBus.Publish(new OnExperienceGainedEvent {
                    Target = ev.Target,
                    Experience = (int) (ev.Experience * ExperienceModifier)
                });
            }
            else if (e.GetType() == typeof(OnExperienceGainedEvent))
            {
                var ev = (OnExperienceGainedEvent) e;

                var experience = ev.Target.Get<ExperienceComponent>();
                if (experience == null) {
                    _world.EventBus.Cancel(e);

                    return;
                }

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
                    health.MaxHealth += Utils.Roll(1, stats.HitDice, stats.ConstitutionModifier);
                    health.CurrentHealth = health.MaxHealth;
                }
            }
        }
    }
}