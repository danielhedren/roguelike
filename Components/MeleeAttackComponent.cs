using roguelike.Utils;

namespace roguelike.Components
{
    public class MeleeAttackComponent : Component
    {
        public int Dice { get; set; } = 1;
        public int Sides { get; set; } = 1;
        public int Modifier { get; set; } = 0;
        public int ToHit { get; set; } = 0;
        public int Damage { get => Random.Dice(Dice, Sides, Modifier); }
        public double Speed { get; set; } = 1;

        public MeleeAttackComponent()
        {
        }

        public MeleeAttackComponent(int dice, int sides, int modifier, int toHit, double speed)
        {
            Dice = dice;
            Sides = sides;
            Modifier = modifier;
            ToHit = ToHit;
            Speed = speed;
        }

        
    }
}