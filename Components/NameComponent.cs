namespace roguelike.Components
{
    public class NameComponent : Component
    {
        public string Name { get; set; }

        public NameComponent(string name)
        {
            Name = name;
        }
    }
}