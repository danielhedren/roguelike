using System;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Components;

namespace roguelike
{
    public class Utils
    {
        public static System.Random RandomGenerator { get; } = new System.Random();
        private static Regex _diceRegex = new Regex("([0-9]*)d([0-9]+)([*+-]?)([0-9]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static int Next(int min, int max)
        {
            return RandomGenerator.Next(min, max);
        }

        public static int Roll(string dice)
        {
            var match = _diceRegex.Match(dice);

            if (!match.Success) return 0;

            int number, sides, modifier;
            if (!int.TryParse(match.Groups[1].Value, out number)) number = 1;
            int.TryParse(match.Groups[2].Value, out sides);

            var roll = Roll(number, sides);

            if (match.Groups[3].Success && int.TryParse(match.Groups[4].Value, out modifier))
            {
                switch (match.Groups[3].Value)
                {
                    case "*":
                        roll *= modifier;
                        break;
                    case "-":
                        roll -= modifier;
                        break;
                    default:
                        roll += modifier;
                        break;
                }
            }

            return roll;
        }

        public static int Roll(int dice = 1, int sides = 6, int modifier = 0)
        {
            var result = 0;

            for (var i = 0; i < dice; i++)
            {
                result += Next(1, sides + 1);
            }

            result += modifier;

            return result;
        }

        public static bool IsNextTo(Point? a, Point? b)
        {
            if (a == null || b == null) return false;

            return (Math.Abs(a.Value.X - b.Value.X) <= 1 && Math.Abs(a.Value.Y - b.Value.Y) <= 1);
        }

        public static double DistanceBetween(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public static double? DistanceBetween(Actor a, Actor b)
        {
            var aPos = a.Get<EntityComponent>();
            var bPos = b.Get<EntityComponent>();

            if (aPos == null || bPos == null) return null;

            return Math.Sqrt(Math.Pow(aPos.X - bPos.X, 2) + Math.Pow(aPos.Y - bPos.Y, 2));
        }

        public static RogueSharp.ICell GetAdjacentWalkable(RogueSharp.Map map, Point point)
        {
            foreach (var cell in map.GetBorderCellsInSquare(point.X, point.Y, 1))
            {
                if (cell.IsWalkable)
                {
                    return cell;
                }
            }

            return null;
        }
    }
}