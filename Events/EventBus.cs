using System.Collections.Generic;
using System.Linq;
using roguelike.Handlers;
using roguelike.World;

namespace roguelike.Events
{
    public class EventBus
    {
        public static List<Event> Events = new List<Event>();
        private static Dictionary<System.Type, List<IHandler>> Subscribers = new Dictionary<System.Type, List<IHandler>>();

        public static bool HandleNext(Level level)
        {
            if (Events.Count == 0) return false;

            Events = Events.OrderBy(x => x.ActivateIn).ToList();
            var e = Events.First();

            if (Subscribers.ContainsKey(e.GetType()))
            {
                foreach (var sub in Subscribers[e.GetType()])
                {
                    sub.HandleEvent(e, level);

                    if (e.StopPropagation) break;
                }
            }

            var tActivateIn = e.ActivateIn;
            foreach (var e2 in Events)
            {
                e2.ActivateIn -= tActivateIn;
            }

            var interrupt = e.Interrupt;

            Events.Remove(e);

            return !interrupt;
        }

        public static void Subscribe(System.Type type, IHandler handler)
        {
            if (!type.IsSubclassOf(typeof(Event))) {
                throw new System.Exception();
            }

            if (!Subscribers.ContainsKey(type)) {
                Subscribers.Add(type, new List<IHandler>());
            }

            Subscribers[type].Add(handler);
        }

        public static void Publish(Event e)
        {
            EventBus.Events.Add(e);
        }
    }
}