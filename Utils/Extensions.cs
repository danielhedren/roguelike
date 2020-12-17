using RogueSharp;

namespace roguelike
{
    public static class Extensions
    {
        public static void SetWalkable(this IMap map, int x, int y, bool isWalkable)
        {
            map.SetCellProperties(x, y, map.IsTransparent(x, y), isWalkable);
        }

        public static void SetTransparent(this IMap map, int x, int y, bool isTransparent)
        {
            map.SetCellProperties(x, y, isTransparent, map.IsWalkable(x, y));
        }

        public static void SetExplored(this IMap map, int x, int y, bool isExplored)
        {
            map.SetCellProperties(x, y, map.IsTransparent(x, y), map.IsWalkable(x, y), isExplored);
        }
    }
}