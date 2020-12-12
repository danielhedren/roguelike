using roguelike.Actors;
using roguelike.World;

namespace roguelike.Components
{
    public class MeleeAttackComponent : Component
    {
        public double Damage { get; set; } = 0;
        public double Speed { get; set; } = 1;

        public MeleeAttackComponent(double damage)
        {
            Damage = damage;
        }
    }
}