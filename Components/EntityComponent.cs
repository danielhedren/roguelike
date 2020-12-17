using Microsoft.Xna.Framework;
using SadConsole.Entities;

namespace roguelike.Components
{
    public class EntityComponent : Component
    {
        public Entity Entity { get; set; }
        public Point Position { get => Entity.Position; set => Entity.Position = value; }
        public int X { get => Entity.Position.X; set => Entity.Position = new Point(value, Entity.Position.Y); }
        public int Y { get => Entity.Position.Y; set => Entity.Position = new Point(Entity.Position.Y, Entity.Position.X); }
        public bool IsWalkable { get; set; } = false;
        public bool StayRevealed { get; set; } = false;

        public EntityComponent() { }
        public EntityComponent(Color foreground, Color background, int glyph)
        {
            Entity = new Entity(foreground, background, glyph);
        }
    }
}