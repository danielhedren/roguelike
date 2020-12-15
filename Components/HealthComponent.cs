namespace roguelike.Components
{
    public class HealthComponent : Component
    {
        public double CurrentHealth { get; set; }
        public double MaxHealth { get; set; }
        public bool IsDead { get => CurrentHealth <= 0; }

        public HealthComponent(double maxHealth)
        {
            maxHealth = maxHealth > 0 ? maxHealth : 1;
            CurrentHealth = MaxHealth = maxHealth;
        }
    }
}