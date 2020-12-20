namespace roguelike.Components
{
    public class ArmorComponent : Component
    {
        public int ArmorClass { get; set; }
        public StatsComponent.Stat Modifier { get; set; }
        public int? MaximumModifier { get; set; } = null;
        public ArmorComponent()
        {

        }
    }
}