using System;
using System.Collections.Generic;
using System.Linq;
using roguelike.Handlers;
using roguelike.Utils;
using roguelike.World;

namespace roguelike.Events
{
    public class EventBus
    {
        public static List<Event> Events { get; set; } = new List<Event>();
        private static Dictionary<System.Type, List<IHandler>> Subscribers { get; set; } = new Dictionary<System.Type, List<IHandler>>();
        private static bool _interrupted { get; set; } = false;

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

            if (!_interrupted) {
                var tActivateIn = e.ActivateIn;
                foreach (var e2 in Events)
                {
                    e2.ActivateIn -= Math.Max(tActivateIn, 0);
                }

                _interrupted = e.Interrupt;
            }

            Events.Remove(e);

            if (_interrupted) {
                Events = Events.OrderBy(x => x.ActivateIn).ToList();
                if (Events.First().ActivateIn <= 0) {
                    return true;
                } else {
                    _interrupted = false;
                    return false;
                }
            }

            return true;
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