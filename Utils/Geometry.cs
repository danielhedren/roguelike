using System;
using Microsoft.Xna.Framework;

namespace roguelike.Utils
{
    public class Geometry
    {
        public static bool IsNextTo(Point a, Point b)
        {
            return (Math.Abs(a.X - b.X) <= 1 && Math.Abs(a.Y - b.Y) <= 1);
        }
    }
}