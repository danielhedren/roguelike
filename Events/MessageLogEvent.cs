using Microsoft.Xna.Framework;

namespace roguelike.Events
{
    public class MessageLogEvent : Event
    {
        public string Scope { get; set; }
        public string Message { get; set; }
        public Color Color { get; set; } = Color.White;
    }
}