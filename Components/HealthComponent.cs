namespace roguelike.Components
{
    public class HealthComponent : Component
    {
        public float CurrentHealth { get; set; }
        public float MaxHealth { get; set; }

        public HealthComponent(float maxHealth)
        {
            CurrentHealth = MaxHealth = maxHealth;
        }
    }
}