namespace roguelike.Components
{
    public class HealthComponent : Component
    {
        public double CurrentHealth { get; set; }
        public double MaxHealth { get; set; }

        public HealthComponent(double maxHealth)
        {
            CurrentHealth = MaxHealth = maxHealth;
        }
    }
}