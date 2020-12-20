using System.Collections.Generic;
using System.Linq;
using roguelike.Components;
using roguelike.Engine;
using roguelike.Events;

namespace roguelike.Handlers
{
    public class ItemHandler : Handler
    {
        public ItemHandler(World world) : base(world)
        {
            Subscribe(typeof(OnItemPickupEvent));
            Subscribe(typeof(BeforeItemEquippedEvent));
        }

        public override void HandleEvent(Event e)
        {
            if (e.GetType() == typeof(OnItemPickupEvent))
            {
                var ev = (OnItemPickupEvent)e;

                _world.EventBus.Publish(new ActorTurnEvent
                {
                    Actor = ev.Target,
                    Interrupt = ev.Target == _world.Player
                });

                if (ev.Target != _world.Player) return;

                var inventory = ev.Target.Get<InventoryComponent>();

                if (inventory == null) return;

                inventory.Items.Add(ev.Item);

                var entity = ev.Item.Get<EntityComponent>();
                if (entity != null)
                {
                    ev.Item.Components.Remove(entity);
                    _world.MapConsole.Console.Children.Remove(entity.Entity);
                }

                var itemC = ev.Item.Get<ItemComponent>();
                if (itemC.Slot != ItemComponent.EquipmentSlot.None)
                {
                    _world.EventBus.Publish(new BeforeItemEquippedEvent
                    {
                        Target = ev.Target,
                        Item = ev.Item
                    });
                }
            }
            else if (e.GetType() == typeof(BeforeItemEquippedEvent))
            {
                var ev = (BeforeItemEquippedEvent)e;

                var inventory = ev.Target.Get<InventoryComponent>();
                var itemC = ev.Item.Get<ItemComponent>();

                if (inventory == null || itemC == null) return;

                if (inventory.EquipmentSlots.ContainsKey(itemC.Slot))
                {
                    inventory.EquipmentSlots[itemC.Slot] = ev.Item;
                }
                else
                {
                    _world.EventBus.Cancel(e);
                    return;
                }

                _world.EventBus.Publish(new OnItemEquippedEvent
                {
                    Target = ev.Target,
                    Item = ev.Item
                });
            }
        }
    }
}