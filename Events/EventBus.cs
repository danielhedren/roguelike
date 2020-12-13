using System;
using System.Collections.Generic;
using System.Linq;
using roguelike.Handlers;
using roguelike.Utils;
using roguelike.Engine;

namespace roguelike.Events
{
    public class EventBus
    {
        public List<Event> Events { get; set; } = new List<Event>();
        private Dictionary<System.Type, List<Handler>> Subscribers { get; set; } = new Dictionary<System.Type, List<Handler>>();
        private List<Handler> Handlers { get; set; } = new List<Handler>();
        private bool _interrupted { get; set; } = false;
        private World _world { get; set; }

        public EventBus(World world)
        {
            _world = world;
        }

        public bool HandleNext()
        {
            if (Events.Count == 0) return false;

            Events = Events.OrderBy(x => x.ActivateIn).ToList();
            var e = Events.First();

            if (Subscribers.ContainsKey(e.GetType()))
            {
                foreach (var sub in Subscribers[e.GetType()])
                {
                    sub.HandleEvent(e);

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
                if (Events.Count() > 0 && Events.First().ActivateIn <= 0) {
                    return true;
                } else {
                    _interrupted = false;
                    return false;
                }
            }

            return true;
        }

        public void Subscribe(System.Type type, Handler handler)
        {
            if (!type.IsSubclassOf(typeof(Event))) {
                throw new System.Exception();
            }

            if (!Subscribers.ContainsKey(type)) {
                Subscribers.Add(type, new List<Handler>());
            }

            Subscribers[type].Add(handler);
        }

        public void Publish(Event e)
        {
            Events.Add(e);
        }

        public void Cancel(Event e)
        {
            if (e.InterruptOnCancel) {
                Publish(new InterruptEvent());
            }
        }

        public void RegisterHandler<T>() where T : Handler
        {
            Handlers.Add((Handler) Activator.CreateInstance(typeof(T), new object[] { _world }));
        }
    }
}