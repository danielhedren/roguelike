using roguelike.Actors;
using roguelike.World;

namespace roguelike.Components
{
    public class MeleeAttackComponent : Component
    {
        public float Damage { get; set; } = 0;

        public MeleeAttackComponent(float damage)
        {
            Damage = damage;
        }
    }
}