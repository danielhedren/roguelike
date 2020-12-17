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

        private class ItemItem
        {
            public Item Item { get; set; }
            public bool Equipped { get; set; }
            public override string ToString()
            {
                var item = Item.Get<ItemComponent>();
                var equippedString = Equipped ? "Equipped" : "";

                return $"{item.Slot,-10}{item.Name,-20}{equippedString,-10}";
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

            _itemListBox = new ListBox(40, Program.Height);
            Console.Add(_itemListBox);

            _itemListBox.SingleClickItemExecute = true;
            _itemListBox.ExclusiveFocus = true;
            _itemListBox.SelectedItemExecuted += (s, e) =>
            {
                World.EventBus.Publish(new BeforeItemEquippedEvent
                {
                    Target = World.Player,
                    Item = ((ItemItem)e.Item).Item
                });

                while (World.EventBus.HandleNext()) { }

                Update();
            };
        }

        public void Update()
        {
            _itemListBox.Items.Clear();

            var inventory = World.Player?.Get<InventoryComponent>();

            if (inventory == null) return;

            foreach (var item in inventory.Items)
            {
                _itemListBox.Items.Add(new ItemItem
                {
                    Item = item,
                    Equipped = inventory.EquippedItems.Contains(item)
                });
            }
        }
    }
}