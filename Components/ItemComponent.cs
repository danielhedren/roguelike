namespace roguelike.Components
{
    public class ItemComponent : Component
    {
        public enum EquipmentSlot
        {
            None,
            Weapon,
            Armor,
            Helmet,
            Shield,
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public EquipmentSlot Slot { get; set; } = EquipmentSlot.None;
        public ItemComponent()
        {

        }
    }
}