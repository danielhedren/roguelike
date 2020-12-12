using System;
using Microsoft.Xna.Framework;

namespace roguelike.Utils
{
    public class Geometry
    {
        public static bool IsNextTo(Point? a, Point? b)
        {
            if (a == null || b == null) return false;

            return (Math.Abs(a.Value.X - b.Value.X) <= 1 && Math.Abs(a.Value.Y - b.Value.Y) <= 1);
        }
    }
}