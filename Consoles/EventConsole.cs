using System;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Actors.Items;
using roguelike.Components;
using roguelike.Engine;
using roguelike.Events;
using SadConsole;
using SadConsole.Controls;

namespace roguelike.Consoles
{
    public class EventConsole : ContainerConsole
    {
        public class EventItem
        {
            public Event Event { get; set; }

            public override string ToString()
            {
                return $"{Event.GetType().Name,-30}{Math.Round(Event.ActivateIn, 2),-10}";
            }
        }
        public ControlsConsole Console { get; set; }
        public World World { get; set; }

        private Button _exitButton { get; set; }
        private Button _handleNextButton { get; set; }
        private Button _handleAllButton { get; set; }
        private ListBox _eventListBox { get; set; }
        private ListBox _propertyListBox { get; set; }

        /*
            TODO: Could change this to inspection console and allow inspection of the event queue, actors, or whatever
        */
        public EventConsole()
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

            _handleNextButton = new Button(15, 3);
            _handleNextButton.Text = "Handle next";
            _handleNextButton.Position = new Point(_exitButton.Position.X - _handleNextButton.Width, _exitButton.Position.Y);
            _handleNextButton.Click += (s, e) =>
            {
                World.EventBus.HandleNext();
                Update();
            };
            Console.Add(_handleNextButton);

            _handleAllButton = new Button(15, 3);
            _handleAllButton.Text = "Handle all";
            _handleAllButton.Position = new Point(_handleNextButton.Position.X - _handleAllButton.Width, _exitButton.Position.Y);
            _handleAllButton.Click += (s, e) =>
            {
                while (World.EventBus.HandleNext()) { }
                Update();
            };
            Console.Add(_handleAllButton);

            _propertyListBox = new ListBox(Program.Width - 43, Program.Height - 5);
            _propertyListBox.Position = new Point(42, 1);
            _propertyListBox.SelectedItemChanged += (s, e) =>
            {

            };
            Console.Add(_propertyListBox);

            /*
                Should make this better, e.g. proper inventory display and so on
            */
            _eventListBox = new ListBox(40, Program.Height - 5);
            _eventListBox.Position = new Point(1, 1);
            _eventListBox.SelectedItemChanged += (s, e) =>
            {
                if (e.Item == null) return;

                _propertyListBox.Items.Clear();

                var ev = (EventItem)e.Item;

                var properties = ev.Event.GetType().GetProperties();

                foreach (var p in properties)
                {
                    var value = p.GetValue(ev.Event);
                    _propertyListBox.Items.Add($"{p.Name} = {value}");

                    if (value.GetType().IsSubclassOf(typeof(Actor)))
                    {
                        var actor = (Actor)value;
                        foreach (var c in actor.Components)
                        {
                            _propertyListBox.Items.Add($"   {c.GetType().Name}");
                            var cP = c.GetType().GetProperties();
                            foreach (var cPP in cP)
                            {
                                _propertyListBox.Items.Add($"       {cPP.Name} = {cPP.GetValue(c)}");
                            }
                        }
                    }
                }
            };
            Console.Add(_eventListBox);
        }

        public void Update()
        {
            _eventListBox.Items.Clear();

            int i = 1;
            foreach (var e in World.EventBus.Events)
            {
                _eventListBox.Items.Add(new EventItem { Event = e });
            }
        }
    }
}