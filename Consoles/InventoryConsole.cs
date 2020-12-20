using Microsoft.Xna.Framework;
using roguelike.Actors.Items;
using roguelike.Components;
using roguelike.Engine;
using roguelike.Events;
using roguelike.Handlers;
using SadConsole;
using SadConsole.Controls;

namespace roguelike.Consoles
{
    public class InventoryConsole : ContainerConsole
    {
        public ControlsConsole Console { get; set; }
        public World World { get; set; }

        private Button _exitButton { get; set; }
        private ListBox _itemListBox { get; set; }
        private ListBox _equippedItemListBox { get; set; }
        private Label _damageLabel { get; set; }
        private Label _acLabel { get; set; }
        private class EquipmentSlotItem
        {
            public Item Item { get; set; }
            public ItemComponent.EquipmentSlot Slot { get; set; }
            public override string ToString()
            {
                if (Item == null) return $"{Slot}";

                var item = Item.Get<ItemComponent>();

                string description = "";
                var weapon = Item.Get<MeleeAttackComponent>();
                var armor = Item.Get<ArmorComponent>();
                if (weapon != null) {
                    var mod = weapon.Modifier != 0 ? $"{weapon.Modifier:+#;-#}" : null;
                    description = $"{weapon.Dice}d{weapon.Sides}{mod}";
                } else if (armor != null) {
                    description = $"AC {armor.ArmorClass} {armor.Modifier,-3}";
                }

                return $"{Slot,-10}{item?.Name,-20}{description,-10}";
            }
        }

        private class ItemItem
        {
            public Item Item { get; set; }
            public bool Equipped { get; set; }
            public override string ToString()
            {
                var item = Item?.Get<ItemComponent>();
                var equippedString = Equipped ? "Equipped" : "";

                return $"{item?.Slot,-10}{item?.Name,-20}{equippedString,-10}";
            }
        }

        public InventoryConsole()
        {
            Console = new ControlsConsole(Program.Width, Program.Height);
            Console.Parent = this;

            _exitButton = new Button(10, 3);
            _exitButton.Text = "Back";
            _exitButton.TextAlignment = HorizontalAlignment.Center;
            _exitButton.Position = new Point(Console.Width - _exitButton.Width, Console.Height - _exitButton.Height);
            _exitButton.Click += (s, e) =>
            {
                SadConsole.Global.CurrentScreen = World.MapConsole;
                SadConsole.Global.CurrentScreen.IsFocused = true;
            };
            Console.Add(_exitButton);

            _equippedItemListBox = new ListBox(40, 10);
            _equippedItemListBox.Position  = new Point(1, 1);
            Console.Add(_equippedItemListBox);

            _itemListBox = new ListBox(40, Program.Height - 13);
            _itemListBox.Position = new Point(1, 12);
            Console.Add(_itemListBox);

            _itemListBox.SingleClickItemExecute = true;
            _itemListBox.SelectedItemExecuted += (s, e) =>
            {
                var item = (ItemItem)e.Item;
                World.EventBus.Publish(new BeforeItemEquippedEvent
                {
                    Target = World.Player,
                    Item = item.Item
                });

                while (World.EventBus.HandleNext()) { }

                Update();
            };

            _damageLabel = new Label(20);
            _damageLabel.Position = new Point(45, 1);
            Console.Add(_damageLabel);

            _acLabel = new Label(20);
            _acLabel.Position = new Point(45, 2);
            Console.Add(_acLabel);
        }

        public void Update()
        {
            _itemListBox.Items.Clear();
            _equippedItemListBox.Items.Clear();

            var inventory = World.Player?.Get<InventoryComponent>();

            if (inventory == null) return;

            foreach (var slot in inventory.EquipmentSlots)
            {
                _equippedItemListBox.Items.Add(new EquipmentSlotItem
                {
                    Item = slot.Value,
                    Slot = slot.Key
                });
            }

            foreach (var item in inventory.Items)
            {
                _itemListBox.Items.Add(new ItemItem
                {
                    Item = item,
                    Equipped = inventory.EquipmentSlots.ContainsValue(item)
                });
            }

            var weapon = AttackHandler.GetWeapon(World.Player)?.Get<MeleeAttackComponent>();
            var stats = World.Player.Get<StatsComponent>();

            if (weapon == null) {
                _damageLabel.DisplayText = $"Damage 1+{stats.StrengthModifier}";
            } else {
                _damageLabel.DisplayText = $"Damage {weapon.Dice}d{weapon.Sides}+{weapon.Modifier + stats.StrengthModifier}";
            }
            _acLabel.DisplayText = $"AC {AttackHandler.GetArmorClass(World.Player)}";
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                SadConsole.Global.CurrentScreen = World.MapConsole;
                SadConsole.Global.CurrentScreen.IsFocused = true;

                return true;
            }

            return false;
        }
    }
}