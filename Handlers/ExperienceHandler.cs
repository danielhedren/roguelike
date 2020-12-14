using roguelike.Components;
using roguelike.Engine;
using roguelike.Events;

namespace roguelike.Handlers
{
    public class ExperienceHandler : Handler
    {
        public ExperienceHandler(World world) : base(world)
        {
            Subscribe(typeof(OnExperienceGainedEvent));
        }

        public override void HandleEvent(Event e)
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
        }
    }
}