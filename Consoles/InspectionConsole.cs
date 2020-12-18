using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using roguelike.Actors;
using roguelike.Actors.Items;
using roguelike.Components;
using roguelike.Engine;
using roguelike.Events;
using SadConsole;
using SadConsole.Controls;
using System.Reflection;

namespace roguelike.Consoles
{
    public class InspectionConsole : ContainerConsole
    {
        public class EventItem
        {
            public Event Event { get; set; }

            public override string ToString()
            {
                return $"{Event.GetType().Name,-30}{Math.Round(Event.ActivateIn, 2),-10}";
            }
        }

        public class ActorItem
        {
            public Actor Actor { get; set; }

            public override string ToString()
            {
                return $"{Actor.GetType().Name,-20}";
            }
        }

        public class PropertyItem
        {
            public object Value { get; set; }
            public string Line { get; set; }
            public override string ToString()
            {
                return Line;
            }
        }

        public ControlsConsole Console { get; set; }
        public World World { get; set; }

        private Button _exitButton { get; set; }
        private Button _handleNextButton { get; set; }
        private Button _handleAllButton { get; set; }
        private ListBox _inspectionItemListBox { get; set; }
        private ListBox _propertyListBox { get; set; }

        /*
            TODO: Could change this to inspection console and allow inspection of the event queue, actors, or whatever
        */
        public InspectionConsole()
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
            _propertyListBox.SelectedItemExecuted += (s, e) =>
            {
                UpdatePropertyList(((PropertyItem)e.Item).Value);
            };
            Console.Add(_propertyListBox);

            /*
                Should make this better, e.g. proper inventory display and so on
            */
            _inspectionItemListBox = new ListBox(40, Program.Height - 5);
            _inspectionItemListBox.Position = new Point(1, 1);
            _inspectionItemListBox.SelectedItemChanged += (s, e) =>
            {
                UpdatePropertyList(e.Item);
            };
            Console.Add(_inspectionItemListBox);
        }

        private void UpdatePropertyList(object obj)
        {
            if (obj == null) return;

            _propertyListBox.Items.Clear();

            var properties = GetProperties(obj);

            foreach (var p in properties)
            {
                _propertyListBox.Items.Add(p);
            }
        }

        private List<PropertyItem> GetProperties(object obj, int recursion = 0)
        {
            string prefix = new string(' ', recursion);

            var recursionLimit = 5;

            var data = new List<PropertyItem>();

            var properties = obj.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var p in properties)
            {
                var indexParameters = p.GetIndexParameters();
                if (indexParameters.Count() > 0)
                {
                    if (indexParameters[0].ParameterType != typeof(Int32)) continue;

                    var len = obj.GetType().GetProperty("Count")?.GetValue(obj);
                    if (len == null) len = obj.GetType().GetProperty("Length")?.GetValue(obj);
                    if (len == null) len = 0;

                    for (var i = 0; i < (int)len; i++)
                    {
                        var val = p.GetValue(obj, new object[] { i });

                        if (recursion < recursionLimit)
                        {
                            data.Add(new PropertyItem { Value = val, Line = $"{prefix}{val.GetType().Name}" });
                            data.AddRange(GetProperties(val, recursion + 1));
                        }
                        else
                        {
                            data.Add(new PropertyItem { Value = val, Line = $"{prefix}{val.GetType().Name} = recursion limit" });
                        }
                    }
                }
                else
                {
                    object val = null;

                    try
                    {
                        val = p.GetValue(obj);
                    }
                    catch (Exception ex)
                    {
                        data.Add(new PropertyItem { Value = null, Line = $"{prefix}{p.Name} threw {ex.GetType().Name}" });
                        continue;
                    }

                    if (val == null) val = "null";

                    if (val.GetType().IsPrimitive || val.GetType() == typeof(string) || recursion == recursionLimit)
                    {
                        data.Add(new PropertyItem { Value = val, Line = $"{prefix}{p.Name} = {val}" });
                    }
                    else
                    {
                        var childProperties = GetProperties(val, recursion + 1);

                        if (childProperties.Count == 0)
                        {
                            data.Add(new PropertyItem { Value = val, Line = $"{prefix}{p.Name} = {val}" });
                        }
                        else
                        {
                            data.Add(new PropertyItem { Value = val, Line = $"{prefix}{p.Name}" });
                            data.AddRange(childProperties);
                        }
                    }
                }
            }

            return data;
        }

        public void Update()
        {
            UpdateEvents();
        }

        public void UpdateEvents()
        {
            _inspectionItemListBox.Items.Clear();

            foreach (var e in World.EventBus.Events)
            {
                _inspectionItemListBox.Items.Add(new EventItem { Event = e });
            }
        }

        public void UpdateActors()
        {
            _inspectionItemListBox.Items.Clear();

            foreach (var a in World.CurrentLevel.Actors)
            {
                _inspectionItemListBox.Items.Add(new ActorItem { Actor = a });
            }
        }
    }
}