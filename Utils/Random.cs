namespace roguelike.Utils
{
    public class Random
    {
        public static System.Random rand { get; } = new System.Random();

        public struct DiceRoll {
            public int Dice;
            public int Sides;
            public int Modifier;
        }

        public static int Next(int min, int max)
        {
            return rand.Next(min, max);
        }

        public static int Dice(int number = 1, int sides = 6, int modifier = 0)
        {
            var result = 0;

            for (var i = 0; i < number; i++)
            {
                result += Next(1, sides + 1);
            }

            result += modifier;

            return result;
        }

        public static int Dice(DiceRoll dice)
        {
            return Dice(dice.Dice, dice.Sides, dice.Modifier);
        }
    }
}