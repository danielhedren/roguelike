namespace roguelike.Components
{
    public class MeleeAttackComponent : Component
    {
        public double Damage { get; set; } = 0;
        public double Speed { get; set; } = 1;

        public MeleeAttackComponent(double damage, double speed = 1)
        {
            Damage = damage;
            Speed = speed;
        }
    }
}