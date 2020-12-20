using Microsoft.Xna.Framework;
using roguelike.Actors.Items;
using roguelike.Components;
using roguelike.Engine;
using roguelike.Events;
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
                if (weapon != null) {
                    var mod = weapon.Modifier != 0 ? $"{weapon.Modifier:+#;-#}" : null;
                    description = $"{weapon.Dice}d{weapon.Sides}{mod}";
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