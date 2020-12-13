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

            Logging.Log($"{ev.GetType()}: {ev.Attacker.GetType()} damaged {ev.Target.GetType()} for {ev.Damage} damage!");

            if (health.IsDead) {
                Logging.Log($"{ev.GetType()}: {ev.Target.GetType()} died!");

                if (ev.Target.GetType() == typeof(Player)) {
                    Logging.Log($"You died!");
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