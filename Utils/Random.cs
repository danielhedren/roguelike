namespace roguelike.Utils
{
    public class Random
    {
        public static System.Random rand { get; } = new System.Random();

        public static int Next(int min, int max)
        {
            return rand.Next(min, max);
        }
    }
}