using roguelike.Actors;
using roguelike.Components;
using roguelike.Engine;
using roguelike.Events;

namespace roguelike.Handlers
{
    public class DeathHandler : Handler
    {
        public DeathHandler(World world) : base(world)
        {
            Subscribe(typeof(OnDeathEvent));
        }

        public override void HandleEvent(Event e)
        {
            var ev = (OnDeathEvent) e;

            var experience = ev.Attacker.Get<ExperienceComponent>();
            if (experience != null) {
                var xpGain = ev.Target.Get<StatsComponent>()?.ExperienceGained ?? 0;

                _world.EventBus.Publish(new BeforeExperienceGainedEvent {
                    Target = ev.Attacker,
                    Experience = xpGain
                });
            }

            var corpse = new Corpse();
            corpse.Get<EntityComponent>().Position = ev.Target.Get<EntityComponent>().Position;
            _world.CurrentLevel.Actors.Add(corpse);

            ev.Target.Components.Clear();
            _world.CurrentLevel.Actors.Remove(ev.Target);
        }
    }
}